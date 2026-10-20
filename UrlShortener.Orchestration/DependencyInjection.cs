using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Orchestration;
using UrlShortener.Orchestration.Governance;

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
