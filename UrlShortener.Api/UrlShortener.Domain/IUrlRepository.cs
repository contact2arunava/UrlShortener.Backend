namespace UrlShortener.Domain
{
    public interface IUrlRepository
    {
        Url Create(string longUrl);
        Url? GetByCode(string code);
    }
}
