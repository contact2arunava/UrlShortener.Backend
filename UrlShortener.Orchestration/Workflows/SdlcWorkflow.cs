using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Domain.Workflow;
using UrlShortener.Service;//Ideally Worflow Orchestration should not depend on Service layer,
                           //but for the sake of using the service interfaces, we will allow it.

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
                    ctx.Data["design"] = "Design completed";

                    //Comment the line above and uncomment the lines below to simulate a design change after review
                    //ctx.Data["design"] = "Design updated after review";
                    //ctx.DirtyNodes.Add("design");
                    return Task.CompletedTask;
                }
            },
            new WorkflowNode
            {
                Id = "implementation",
                DependsOn = { "design" },
                Execute = ctx =>
                {
                    ctx.Data["code"] = "Code generated";
                    return Task.CompletedTask;
                }
            },
            new WorkflowNode
            {
                Id = "unit-tests",
                DependsOn = { "implementation" },
                RunInParallel = true,
                Execute = async ctx =>
                {
                    var tester = ctx.Services.GetRequiredService<ITestRunnerService>();
                    await tester.RunUnitTestsAsync();
                }
            },
            new WorkflowNode
            {
                Id = "integration-tests",
                DependsOn = { "implementation" },
                RunInParallel = true,
                Execute = async ctx =>
                {
                    var tester = ctx.Services.GetRequiredService<ITestRunnerService>();
                    await tester.RunIntegrationTestsAsync();
                }
            },
            new WorkflowNode
            {
                Id = "documentation",
                DependsOn = { "unit-tests", "integration-tests" },
                RunInParallel = false,
                Execute = async ctx =>
                {
                    var docs = ctx.Services.GetRequiredService<IDocumentationService>();
                    var code = (string)ctx.Data["code"];
                    ctx.Data["documentation"] = await docs.GenerateDocumentationAsync(code);
                }
            }

        };
    }
}
