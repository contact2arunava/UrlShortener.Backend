using UrlShortener.Data.Interfaces;
using UrlShortener.Domain;

namespace UrlShortener.Service;

public class UrlService : IUrlService
{
    private readonly IUrlRepository _repo;

    public UrlService(IUrlRepository repo)
    {
        _repo = repo;
    }

    public Url CreateShortUrl(string longUrl)
    {
        return _repo.Create(longUrl);
    }

    public Url? GetByCode(string code)
    {
        return _repo.GetByCode(code);
    }

    public Url? GetById(Guid id)
    {
        return _repo.GetById(id);
    }
}
