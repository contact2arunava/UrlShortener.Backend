namespace UrlShortener.Service;

public class DocumentationService : IDocumentationService
{
    public Task<string> GenerateDocumentationAsync(string code)
    {
        return Task.FromResult($"Generated documentation for code:\n{code}");
    }
}
