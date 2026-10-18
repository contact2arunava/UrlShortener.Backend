using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Api.Data;
using UrlShortener.Data.Interfaces;

namespace UrlShortener.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IUrlRepository, DapperUrlRepository>();
        services.AddScoped<IUrlHitRepository, DapperUrlHitRepository>();
        return services;
    }
}
