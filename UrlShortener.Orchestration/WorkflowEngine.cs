using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UrlShortener.Domain.Workflow;
using UrlShortener.Workflow.Orchestration.Governance;

namespace UrlShortener.Workflow.Orchestration
{
    public class WorkflowEngine
    {
        private readonly ILogger<WorkflowEngine> _logger;
        private readonly IApprovalService _approvalService;
        private readonly IWorkflowRunRepository _repo;

        public WorkflowEngine(
            ILogger<WorkflowEngine> logger,
            IApprovalService approvalService,
            IWorkflowRunRepository repo)
        {
            _logger = logger;
            _approvalService = approvalService;
            _repo = repo;
        }

        /// <summary>
        /// Runs a workflow against an existing persisted WorkflowRun.
        /// Controller creates the run (via IWorkflowRunRepository.CreateAsync)
        /// and passes the runId into this method.
        /// </summary>
        public async Task RunAsync(
            Guid workflowRunId, 
            IReadOnlyList<WorkflowNode> workflow, 
            WorkflowContext context)
        {
            var run = await _repo.GetAsync(workflowRunId);
            if (run == null)
            {
                throw new InvalidOperationException($"WorkflowRun '{workflowRunId}' not found.");
            }

            var nodesById = workflow.ToDictionary(n => n.Id);
            var completed = new HashSet<string>();
            var failed = new HashSet<string>();

            // Simple dependency graph execution: keep running until all nodes are completed or failed
            while (completed.Count + failed.Count < workflow.Count)
            {
                var readyNodes = workflow
                    .Where(n =>
                        !completed.Contains(n.Id) &&
                        !failed.Contains(n.Id) &&
                        n.DependsOn.All(d => completed.Contains(d)))
                    .ToList();

                if (!readyNodes.Any())
                {
                    // No nodes ready but not all completed → deadlock or unmet dependency
                    _logger.LogWarning("No ready nodes found; workflow may be stuck.");
                    break;
                }

                //foreach (var node in readyNodes)
                //{
                //    await ExecuteNodeAsync(node, context, run);
                //    completed.Add(node.Id);
                //}

                // Split ready nodes into parallel and sequential
                var parallelNodes = readyNodes.Where(n => n.RunInParallel).ToList();
                var sequentialNodes = readyNodes.Where(n => !n.RunInParallel).ToList();

                // Run parallel nodes
                if (parallelNodes.Any())
                {
                    var tasks = parallelNodes.Select(async node =>
                    {
                        await ExecuteNodeAsync(node, context, run);
                        completed.Add(node.Id);
                    });

                    await Task.WhenAll(tasks);
                }

                // Run sequential nodes
                foreach (var node in sequentialNodes)
                {
                    await ExecuteNodeAsync(node, context, run);
                    completed.Add(node.Id);
                }
            }

            run.IsCompleted = true;
            run.CompletedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(run);
        }

        private async Task ExecuteNodeAsync(WorkflowNode node, WorkflowContext context, WorkflowRun run)
        {
            _logger.LogInformation("Starting node {NodeId}", node.Id);

            run.CurrentNode = node.Id;
            run.IsWaitingForApproval = node.RequiresApproval;
            await _repo.UpdateAsync(run);

            // If this node requires approval, wait for it
            if (node.RequiresApproval)
            {
                _logger.LogInformation("Waiting for approval for node {NodeId}", node.Id);
                await _approvalService.WaitForApprovalAsync(node.Id);
                run.IsWaitingForApproval = false;
                await _repo.UpdateAsync(run);
            }

            var attempts = 0;
            var maxRetries = node.MaxRetries;

            while (true)
            {
                try
                {
                    await node.Execute(context);

                    var evt = new WorkflowEvent
                    {
                        Id = Guid.NewGuid(),
                        WorkflowRunId = run.Id,
                        NodeId = node.Id,
                        Timestamp = DateTime.UtcNow,
                        Message = "Completed",
                        Error = null
                    };

                    await _repo.AddEventAsync(evt);

                    _logger.LogInformation("Node {NodeId} completed", node.Id);
                    break;
                }
                catch (Exception ex)
                {
                    attempts++;

                    var evt = new WorkflowEvent
                    {
                        Id = Guid.NewGuid(),
                        WorkflowRunId = run.Id,
                        NodeId = node.Id,
                        Timestamp = DateTime.UtcNow,
                        Message = "Failed",
                        Error = ex.Message
                    };

                    await _repo.AddEventAsync(evt);

                    _logger.LogError(ex, "Node {NodeId} failed on attempt {Attempt}", node.Id, attempts);

                    if (attempts > maxRetries)
                    {
                        // Run rollback if defined
                        if (node.Rollback != null)
                        {
                            _logger.LogInformation("Running rollback for node {NodeId}", node.Id);
                            await node.Rollback(context);

                            var rollbackEvt = new WorkflowEvent
                            {
                                Id = Guid.NewGuid(),
                                WorkflowRunId = run.Id,
                                NodeId = node.Id,
                                Timestamp = DateTime.UtcNow,
                                Message = "RolledBack",
                                Error = null
                            };

                            await _repo.AddEventAsync(rollbackEvt);
                        }

                        // After rollback, rethrow or mark as failed; here we just stop this node
                        _logger.LogWarning("Node {NodeId} exceeded max retries ({MaxRetries})", node.Id, maxRetries);
                        break;
                    }

                    // Retry loop continues
                }
            }
        }
    }
 
}
