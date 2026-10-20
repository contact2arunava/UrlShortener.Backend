namespace UrlShortener.Service;

public class TestRunnerService : ITestRunnerService
{
    public Task<bool> RunUnitTestsAsync()
    {
        // Simulate passing tests
        return Task.FromResult(true);
    }

    public Task<bool> RunIntegrationTestsAsync()
    {
        // Simulate passing tests
        return Task.FromResult(true);
    }

    public Task<bool> RunAllTestsAsync()
    {
        return Task.FromResult(true);
    }
}
