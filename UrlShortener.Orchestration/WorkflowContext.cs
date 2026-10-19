namespace UrlShortener.Orchestration;

public class WorkflowContext
{
    public Dictionary<string, object> Data { get; } = new();
    public List<WorkflowEvent> Events { get; } = new();
}
