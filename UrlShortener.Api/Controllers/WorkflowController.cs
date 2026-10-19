using Microsoft.AspNetCore.Mvc;
using UrlShortener.Orchestration;
using UrlShortener.Orchestration.Governance;
using UrlShortener.Orchestration.Workflows;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/workflows")]
public class WorkflowController : ControllerBase
{
    private readonly WorkflowEngine _engine;

    public WorkflowController(WorkflowEngine engine)
    {
        _engine = engine;
    }

    [HttpPost("sdlc")]
    public async Task<IActionResult> RunSdlcWorkflow()
    {
        var workflow = SdlcWorkflow.Create();
        var context = new WorkflowContext();

        await _engine.RunAsync(workflow, context);

        return Ok(context.Events);
    }

    [HttpPost("approve/{nodeId}")]
    public IActionResult ApproveNode(string nodeId, [FromServices] IApprovalService approvalService)
    {
        approvalService.Approve(nodeId);
        return Ok(new { message = $"Node '{nodeId}' approved." });
    }

    [HttpPost("reject/{nodeId}")]
    public IActionResult RejectNode(string nodeId, [FromServices] IApprovalService approvalService)
    {
        approvalService.Reject(nodeId);
        return Ok(new { message = $"Node '{nodeId}' approved." });
    }

}
