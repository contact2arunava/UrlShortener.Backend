namespace UrlShortener.Service;

public interface IDocumentationService
{
    Task<string> GenerateDocumentationAsync(string code);
}
