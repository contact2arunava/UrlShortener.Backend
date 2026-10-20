namespace UrlShortener.Domain.Workflow;

public class WorkflowEvent
{
    public Guid Id { get; set; }
    public Guid WorkflowRunId { get; set; }
    public string NodeId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Error { get; set; }
}

