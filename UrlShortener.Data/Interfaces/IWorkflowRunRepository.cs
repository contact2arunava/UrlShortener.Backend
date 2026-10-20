public interface IWorkflowRunRepository
{
    Task<WorkflowRun> CreateAsync();
    Task UpdateAsync(WorkflowRun run);
    Task AddEventAsync(WorkflowEvent evt);
    Task<WorkflowRun?> GetAsync(Guid id);
}
