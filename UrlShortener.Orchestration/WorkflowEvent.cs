namespace UrlShortener.Orchestration;

public class WorkflowEvent
{
    public string NodeId { get; set; } = default!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Message { get; set; } = default!;
    public string? Error { get; set; }
}
