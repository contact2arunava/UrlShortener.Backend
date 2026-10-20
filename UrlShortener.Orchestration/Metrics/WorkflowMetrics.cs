namespace UrlShortener.Workflow.Orchestration.Metrics;

public class WorkflowMetrics
{
    public int TotalNodes { get; set; }
    public int SuccessfulNodes { get; set; }
    public int FailedNodes { get; set; }
    public int RetryCount { get; set; }
    public int RollbackCount { get; set; }
    public TimeSpan TotalDuration { get; set; }
}
