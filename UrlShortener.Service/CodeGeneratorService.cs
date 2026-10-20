namespace UrlShortener.Service;

public class CodeGeneratorService : ICodeGeneratorService
{
    public Task<string> GenerateCodeAsync(string requirements)
    {
        // For now, return dummy code
        return Task.FromResult($"// Generated code for: {requirements}");
    }
}
