using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IUrlService, UrlService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        return services;
    }
}
