using UrlShortener.Domain;

namespace UrlShortener.Service;

public interface IUrlService
{
    Url CreateShortUrl(string longUrl);
    Url? GetByCode(string code);
    Url? GetById(Guid id);
}
