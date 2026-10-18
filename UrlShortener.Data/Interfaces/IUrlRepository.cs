using UrlShortener.Domain;

namespace UrlShortener.Data.Interfaces
{
    public interface IUrlRepository
    {
        Url Create(string longUrl);
        Url? GetByCode(string code);
        Url? GetById(Guid id);

    }
}
