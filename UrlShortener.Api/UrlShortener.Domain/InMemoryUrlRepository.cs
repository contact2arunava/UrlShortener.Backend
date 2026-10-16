using System.Collections.Concurrent;
namespace UrlShortener.Domain
{
    public class InMemoryUrlRepository : IUrlRepository
    {
        private readonly ConcurrentDictionary<string, Url> _store = new();

        public Url Create(string longUrl)
        {
            var code = GenerateCode();
            var url = new Url
            {
                Code = code,
                LongUrl = longUrl
            };

            _store[code] = url;
            return url;
        }

        public Url? GetByCode(string code)
        {
            _store.TryGetValue(code, out var url);
            return url;
        }

        private string GenerateCode()
        {
            return Guid.NewGuid().ToString("N")[..8];
        }
    }
}
