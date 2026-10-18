namespace UrlShortener.Domain;

public class UrlHit
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UrlId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? UserAgent { get; set; }
    public string? IpHash { get; set; }
}
