using System.Collections.Concurrent;
using UrlShortener.Data.Interfaces;
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

        public Url? GetById(Guid id)
        {
            return _store.Values.FirstOrDefault(url => url.Id == id);
        }



        private string GenerateCode()
        {
            return Guid.NewGuid().ToString("N")[..8];
        }
    }
}
