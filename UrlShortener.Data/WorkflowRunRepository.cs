using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using UrlShortener.Domain;

public class WorkflowRunRepository : IWorkflowRunRepository
{
    private readonly string _connectionString;

    public WorkflowRunRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("UrlShortener");
    }

    public async Task<WorkflowRun> CreateAsync()
    {
        var run = new WorkflowRun
        {
            Id = Guid.NewGuid(),
            StartedAt = DateTime.UtcNow,
            IsCompleted = false,
            CurrentNode = "",
            IsWaitingForApproval = false
        };

        using var db = new SqlConnection(_connectionString);

        await db.ExecuteAsync(
            @"INSERT INTO WorkflowRuns 
              (Id, StartedAt, IsCompleted, CurrentNode, IsWaitingForApproval)
              VALUES (@Id, @StartedAt, @IsCompleted, @CurrentNode, @IsWaitingForApproval)",
            run);

        return run;
    }

    public async Task UpdateAsync(WorkflowRun run)
    {
        using var db = new SqlConnection(_connectionString);

        await db.ExecuteAsync(
            @"UPDATE WorkflowRuns
              SET CompletedAt = @CompletedAt,
                  IsCompleted = @IsCompleted,
                  CurrentNode = @CurrentNode,
                  IsWaitingForApproval = @IsWaitingForApproval
              WHERE Id = @Id",
            run);
    }

    public async Task AddEventAsync(WorkflowEvent evt)
    {
        using var db = new SqlConnection(_connectionString);

        await db.ExecuteAsync(
            @"INSERT INTO WorkflowEvents 
              (Id, WorkflowRunId, NodeId, Timestamp, Message, Error)
              VALUES (@Id, @WorkflowRunId, @NodeId, @Timestamp, @Message, @Error)",
            evt);
    }

    public async Task<WorkflowRun?> GetAsync(Guid id)
    {
        using var db = new SqlConnection(_connectionString);

        var run = await db.QueryFirstOrDefaultAsync<WorkflowRun>(
            @"SELECT * FROM WorkflowRuns WHERE Id = @Id",
            new { Id = id });

        if (run == null)
            return null;

        var events = await db.QueryAsync<WorkflowEvent>(
            @"SELECT * FROM WorkflowEvents 
              WHERE WorkflowRunId = @Id 
              ORDER BY Timestamp",
            new { Id = id });

        run.Events = events.ToList();
        return run;
    }
}
