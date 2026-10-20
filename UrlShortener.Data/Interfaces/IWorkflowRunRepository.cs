using UrlShortener.Domain.Workflow;
public interface IWorkflowRunRepository
{
    Task<WorkflowRun> CreateAsync();
    Task UpdateAsync(WorkflowRun run);
    Task AddEventAsync(WorkflowEvent evt);
    Task<WorkflowRun?> GetAsync(Guid id);
    Task PauseAsync(Guid id, string reason);
    Task ResumeAsync(Guid id);
    Task CancelAsync(Guid id, string reason);

}
