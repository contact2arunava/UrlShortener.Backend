using UrlShortener.Domain;

namespace UrlShortener.Service;

public interface IAnalyticsService
{
    void LogHit(Guid urlId, string? userAgent, string? ipHash);
    IEnumerable<UrlHit> GetHits(Guid urlId);
}
