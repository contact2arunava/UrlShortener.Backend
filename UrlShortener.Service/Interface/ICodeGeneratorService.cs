namespace UrlShortener.Service;

public interface ICodeGeneratorService
{
    Task<string> GenerateCodeAsync(string requirements);
}
