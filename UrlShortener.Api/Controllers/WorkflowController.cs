using Microsoft.AspNetCore.Mvc;
using UrlShortener.Domain.Workflow;
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

    [HttpPost("sdlc/start")]
    public async Task<IActionResult> Start([FromServices] IWorkflowRunRepository repo)
    {
        var run = await repo.CreateAsync();
        return Ok(new { workflowId = run.Id });
    }

    [HttpPost("sdlc/run/{workflowId}")]
    public async Task<IActionResult> Run(
    Guid workflowId,
    [FromServices] WorkflowEngine engine)
    {
        // 1. Build the workflow definition
        var workflow = SdlcWorkflow.Create();

        // 2. Build the workflow context (inject DI services)
        var context = new WorkflowContext
        {
            Services = HttpContext.RequestServices
        };

        // 3. Run the workflow
        await engine.RunAsync(workflowId, workflow, context);

        return Ok(new { message = "Workflow executed" });
    }

    [HttpPost("approve/{nodeId}")]
    public IActionResult ApproveNode(string nodeId,
    [FromServices] IApprovalService approvalService)
    {
        approvalService.Approve(nodeId);
        return Ok(new { message = $"Node '{nodeId}' approved." });
    }


    [HttpPost("reject/{nodeId}")]
    public IActionResult RejectNode(string nodeId,
    [FromServices] IApprovalService approvalService)
    {
        approvalService.Reject(nodeId);
        return Ok(new { message = $"Node '{nodeId}' rejected." });
    }





    //[HttpPost("approve/{nodeId}")]
    //public IActionResult ApproveNode(string nodeId, [FromServices] IApprovalService approvalService)
    //{
    //    approvalService.Approve(nodeId);
    //    return Ok(new { message = $"Node '{nodeId}' approved." });
    //}

    //[HttpPost("reject/{nodeId}")]
    //public IActionResult RejectNode(string nodeId, [FromServices] IApprovalService approvalService)
    //{
    //    approvalService.Reject(nodeId);
    //    return Ok(new { message = $"Node '{nodeId}' approved." });
    //}

    //[HttpPost("{workflowId}/run")]
    //public async Task<IActionResult> Run(Guid workflowId,
    //[FromServices] WorkflowRunStore store,
    //[FromServices] WorkflowEngine engine)
    //{
    //    var run = store.Get(workflowId);
    //    if (run == null) return NotFound();

    //    var workflow = SdlcWorkflow.Create();

    //    await engine.RunAsync(workflow, run.Context);

    //    run.IsCompleted = true;

    //    return Ok(run.Context.Events);
    //}

    //[HttpGet("{workflowId}/status")]
    //public IActionResult Status(Guid workflowId, [FromServices] WorkflowRunStore store)
    //{
    //    var run = store.Get(workflowId);
    //    if (run == null) return NotFound();

    //    return Ok(new
    //    {
    //        run.Id,
    //        run.IsCompleted,
    //        run.IsWaitingForApproval,
    //        run.CurrentNode,
    //        Events = run.Context.Events
    //    });
    //}



}
