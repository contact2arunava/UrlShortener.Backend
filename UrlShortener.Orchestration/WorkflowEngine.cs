using Microsoft.Extensions.Logging;
using UrlShortener.Orchestration.Governance;

namespace UrlShortener.Orchestration;

public class WorkflowEngine
{
    private readonly ILogger<WorkflowEngine> _logger;
    private readonly IApprovalService _approvalService;

    public WorkflowEngine(ILogger<WorkflowEngine> logger, IApprovalService approvalService)
    {
        _logger = logger;
        _approvalService = approvalService;
    }

    public async Task RunAsync(List<WorkflowNode> nodes, WorkflowContext context)
    {
        var pending = nodes.ToDictionary(n => n.Id, n => n);
        var completed = new HashSet<string>();

        while (pending.Any())
        {
            var ready = pending.Values
                .Where(n => n.DependsOn.All(d => completed.Contains(d)))
                .ToList();

            foreach (var node in ready)
            {
                if (node.RequiresApproval)
                {
                    await _approvalService.WaitForApprovalAsync(node.Id);
                }

                int attempts = 0;

                while (attempts <= node.MaxRetries)
                {
                    try
                    {
                        _logger.LogInformation("Executing node {NodeId}", node.Id);
                        await node.Execute(context);

                        context.Events.Add(new WorkflowEvent
                        {
                            NodeId = node.Id,
                            Message = "Completed"
                        });

                        completed.Add(node.Id);
                        pending.Remove(node.Id);
                        break;
                    }
                    catch (Exception ex)
                    {
                        attempts++;

                        context.Events.Add(new WorkflowEvent
                        {
                            NodeId = node.Id,
                            Message = "Failed",
                            Error = ex.Message
                        });

                        _logger.LogError(ex, "Node {NodeId} failed attempt {Attempt}", node.Id, attempts);

                        if (attempts > node.MaxRetries)
                        {
                            if (node.Rollback != null)
                            {
                                _logger.LogWarning("Rolling back node {NodeId}", node.Id);
                                await node.Rollback(context);
                            }

                            throw;
                        }
                    }
                }
            }
        }
    }
}
