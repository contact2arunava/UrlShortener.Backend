using UrlShortener.Domain.Workflow;

namespace UrlShortener.Workflow.Orchestration.Workflows;

public static class SdlcWorkflow
{
    public static List<WorkflowNode> Create()
    {
        return new List<WorkflowNode>
        {
            new WorkflowNode
            {
                Id = "requirements",
                Execute = ctx =>
                {
                    ctx.Data["requirements"] = "Parsed";
                    return Task.CompletedTask;
                }
            },
            new WorkflowNode
            {
                Id = "design",
                DependsOn = { "requirements" },
                RequiresApproval = true,
                Execute = ctx =>
                {
                    ctx.Data["design"] = "Completed";
                    return Task.CompletedTask;
                }
            },
            new WorkflowNode
            {
                Id = "implementation",
                DependsOn = { "design" },
                Execute = ctx =>
                {
                    ctx.Data["code"] = "Generated";
                    return Task.CompletedTask;
                }
            },
            new WorkflowNode
            {
                Id = "testing",
                DependsOn = { "implementation" },
                Execute = ctx =>
                {
                    ctx.Data["tests"] = "Passed";
                    return Task.CompletedTask;
                }
            },
            new WorkflowNode
            {
                Id = "documentation",
                DependsOn = { "testing" },
                Execute = ctx =>
                {
                    ctx.Data["docs"] = "Generated";
                    return Task.CompletedTask;
                }
            }
        };
    }
}
