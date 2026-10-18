using UrlShortener.Domain;

namespace UrlShortener.Data.Interfaces
{
    public interface IUrlHitRepository
    {
        void LogHit(Guid urlId, string? userAgent, string? ipHash);
        IEnumerable<UrlHit> GetHits(Guid urlId);
    }
}
