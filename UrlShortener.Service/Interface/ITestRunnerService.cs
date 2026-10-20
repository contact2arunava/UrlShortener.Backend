namespace UrlShortener.Service;

public interface ITestRunnerService
{
    Task<bool> RunUnitTestsAsync();
    Task<bool> RunIntegrationTestsAsync();
    Task<bool> RunAllTestsAsync();
}
