namespace UrlShortener.Domain.Workflow;

public class WorkflowRun
{
    public Guid Id { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsCompleted { get; set; }
    public string CurrentNode { get; set; } = string.Empty;
    public bool IsWaitingForApproval { get; set; }

    public List<WorkflowEvent> Events { get; set; } = new();
}
