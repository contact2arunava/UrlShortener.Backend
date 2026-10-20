using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IUrlService, UrlService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<ITestRunnerService, TestRunnerService>();
        services.AddScoped<ICodeGeneratorService, CodeGeneratorService>();
        services.AddScoped<IDocumentationService, DocumentationService>();
        services.AddScoped<IWorkflowService, WorkflowService>();
        return services;
    }
}
