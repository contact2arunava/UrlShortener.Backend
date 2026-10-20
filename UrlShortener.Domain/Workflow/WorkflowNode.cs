namespace UrlShortener.Domain.Workflow;

public class WorkflowNode
{
    public string Id { get; set; } = default!;
    public Func<WorkflowContext, Task> Execute { get; set; } = default!;
    public Func<WorkflowContext, Task>? Rollback { get; set; }
    public List<string> DependsOn { get; set; } = new();
    public bool RequiresApproval { get; set; }
    public int MaxRetries { get; set; } = 3;
    //mark nodes that can run in parallel
    public bool RunInParallel { get; set; } = false;
}
