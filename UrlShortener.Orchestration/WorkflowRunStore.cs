namespace UrlShortener.Orchestration;

public class WorkflowRunStore
{
    private readonly Dictionary<Guid, WorkflowRun> _runs = new();

    public WorkflowRun CreateRun()
    {
        var run = new WorkflowRun();
        _runs[run.Id] = run;
        return run;
    }

    public WorkflowRun? Get(Guid id)
    {
        return _runs.TryGetValue(id, out var run) ? run : null;
    }
}
