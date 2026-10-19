namespace UrlShortener.Orchestration.Governance;

public interface IApprovalService
{
    Task WaitForApprovalAsync(string nodeId);
    void Approve(string nodeId);
    void Reject(string nodeId, Exception? ex = null);
}
