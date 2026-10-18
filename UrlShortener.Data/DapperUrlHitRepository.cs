using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UrlShortener.Data.Interfaces;
using UrlShortener.Domain;

namespace UrlShortener.Api.Data;

public class DapperUrlHitRepository:IUrlHitRepository
{
    private readonly string _connectionString;

    public DapperUrlHitRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("UrlShortener")
            ?? throw new InvalidOperationException("Missing connection string");
    }

    public void LogHit(Guid urlId, string? userAgent, string? ipHash)
    {
        var hit = new UrlHit
        {
            UrlId = urlId,
            UserAgent = userAgent,
            IpHash = ipHash
        };

        using var conn = new SqlConnection(_connectionString);

        const string sql = @"
            INSERT INTO UrlHits (Id, UrlId, Timestamp, UserAgent, IpHash)
            VALUES (@Id, @UrlId, @Timestamp, @UserAgent, @IpHash);
        ";

        conn.Execute(sql, hit);
    }

    public IEnumerable<UrlHit> GetHits(Guid urlId)
    {
        using var conn = new SqlConnection(_connectionString);

        const string sql = @"
            SELECT Id, UrlId, Timestamp, UserAgent, IpHash
            FROM UrlHits
            WHERE UrlId = @UrlId
            ORDER BY Timestamp DESC;
        ";

        return conn.Query<UrlHit>(sql, new { UrlId = urlId });
    }
}
