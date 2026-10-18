using UrlShortener.Data.Interfaces;
using UrlShortener.Domain;

namespace UrlShortener.Service;

public class AnalyticsService : IAnalyticsService
{
    private readonly IUrlHitRepository _hitRepo;

    public AnalyticsService(IUrlHitRepository hitRepo)
    {
        _hitRepo = hitRepo;
    }

    public void LogHit(Guid urlId, string? userAgent, string? ipHash)
    {
        _hitRepo.LogHit(urlId, userAgent, ipHash);
    }

    public IEnumerable<UrlHit> GetHits(Guid urlId)
    {
        return _hitRepo.GetHits(urlId);
    }
}
