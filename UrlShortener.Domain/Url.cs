namespace UrlShortener.Domain
{
    public class Url
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Code { get; set; } = default!;
        public string LongUrl { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
