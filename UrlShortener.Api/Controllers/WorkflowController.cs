using Microsoft.AspNetCore.Mvc;
using UrlShortener.Domain.Workflow;
using UrlShortener.Service;
using UrlShortener.Workflow.Orchestration;
using UrlShortener.Workflow.Orchestration.Governance;
using UrlShortener.Workflow.Orchestration.Workflows;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/workflows")]
public class WorkflowController : ControllerBase
{
    private readonly WorkflowEngine _engine;
    private readonly IWorkflowService _workflowService;
    private readonly IApprovalService _approvalService;

    public WorkflowController(
        WorkflowEngine engine, 
        IWorkflowService workflowService,
        IApprovalService approvalService)
    {
        _engine = engine;
        _workflowService = workflowService;
        _approvalService = approvalService;
    }

    [HttpPost("sdlc/start")]
    public async Task<IActionResult> Start([FromServices] IWorkflowRunRepository repo)
    {
        var run = await repo.CreateAsync();
        return Ok(new { workflowId = run.Id });
    }

    [HttpPost("sdlc/run/{workflowId}")]
    public async Task<IActionResult> Run(
    Guid workflowId)
    {
        // 1. Build the workflow definition
        var workflow = SdlcWorkflow.Create();

        // 2. Build the workflow context (inject DI services)
        var context = new WorkflowContext
        {
            Services = HttpContext.RequestServices
        };

        // 3. Run the workflow
        await _engine.RunAsync(workflowId, workflow, context);

        return Ok(new { message = "Workflow executed" });
    }

    [HttpPost("approve/{nodeId}")]
    public IActionResult ApproveNode(string nodeId)
    {
        _approvalService.Approve(nodeId);
        return Ok(new { message = $"Node '{nodeId}' approved." });
    }


    [HttpPost("reject/{nodeId}")]
    public IActionResult RejectNode(string nodeId)
    {
        _approvalService.Reject(nodeId);
        return Ok(new { message = $"Node '{nodeId}' rejected." });
    }

    [HttpPost("{id}/pause")]
    public async Task<IActionResult> Pause(
        Guid id, 
        [FromBody] string reason)
    {
        await _workflowService.PauseAsync(id, reason);
        return Ok(new { Status = "Paused", Reason = reason });
    }

    [HttpPost("{id}/resume")]
    public async Task<IActionResult> Resume(Guid id)
    {
        await _workflowService.ResumeAsync(id);
        return Ok(new { Status = "Resumed" });
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, 
        [FromBody] string reason)
    {
        await _workflowService.CancelAsync(id, reason);
        return Ok(new { Status = "Canceled", Reason = reason });
    }

}
