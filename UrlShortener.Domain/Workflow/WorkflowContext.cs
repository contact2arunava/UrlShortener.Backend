namespace UrlShortener.Domain.Workflow;

public class WorkflowContext
{
    // Allows workflow nodes to resolve DI services
    public IServiceProvider Services { get; set; } = default!;

    // Arbitrary data shared between nodes
    public Dictionary<string, object> Data { get; } = new();

    // In-memory event list (DB events are stored separately)
    public List<WorkflowEvent> Events { get; } = new();

    // Nodes whose outputs have changed
    public HashSet<string> DirtyNodes { get; } = new();
}
