using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Workflow.Orchestration;
using UrlShortener.Workflow.Orchestration.Governance;

public static class DependencyInjection
{
    public static IServiceCollection AddOrchestrationLayer(this IServiceCollection services)
    {
        services.AddSingleton<IApprovalService, ManualApprovalService>();
        services.AddSingleton<WorkflowRunStore>();
        services.AddScoped<WorkflowEngine>();
        return services;
    }
}
