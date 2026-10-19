using System.Collections.Concurrent;

namespace UrlShortener.Orchestration.Governance;

public class ManualApprovalService : IApprovalService
{
    private readonly ConcurrentDictionary<string, TaskCompletionSource<bool>> _approvals = new();

    public Task WaitForApprovalAsync(string nodeId)
    {
        var tcs = _approvals.GetOrAdd(
            nodeId,
            _ => new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously)
        );

        return tcs.Task;
    }

    public void Approve(string nodeId)
    {
        if (_approvals.TryRemove(nodeId, out var tcs))
        {
            tcs.TrySetResult(true);
        }
    }

    // Optional helper to reject/cancel approvals
    public void Reject(string nodeId, Exception? ex = null)
    {
        if (_approvals.TryRemove(nodeId, out var tcs))
        {
            if (ex is null) tcs.TrySetCanceled();
            else tcs.TrySetException(ex);
        }
    }
}
