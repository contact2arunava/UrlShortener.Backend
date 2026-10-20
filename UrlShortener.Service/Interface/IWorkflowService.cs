namespace UrlShortener.Service;

public interface IWorkflowService
{
    Task PauseAsync(Guid id, string reason);
    Task ResumeAsync(Guid id);
    Task CancelAsync(Guid id, string reason);
}
