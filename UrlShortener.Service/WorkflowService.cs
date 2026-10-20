namespace UrlShortener.Service
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowRunRepository _repo;

        public WorkflowService(IWorkflowRunRepository repo)
        {
            _repo = repo;
        }

        public Task PauseAsync(Guid id, string reason)
            => _repo.PauseAsync(id, reason);

        public Task ResumeAsync(Guid id)
            => _repo.ResumeAsync(id);

        public Task CancelAsync(Guid id, string reason)
            => _repo.CancelAsync(id, reason);
    }

}
