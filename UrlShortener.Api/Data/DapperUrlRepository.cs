using System.Data;
using Microsoft.Data.SqlClient;    
using Dapper;
using UrlShortener.Domain;


namespace UrlShortener.Api.Data
{
    public class DapperUrlRepository : IUrlRepository
    {
        private readonly string _connectionString;

        public DapperUrlRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("UrlShortener")
                ?? throw new InvalidOperationException("Missing connection string");
        }

        public Url Create(string longUrl)
        {
            var url = new Url
            {
                Code = GenerateCode(),
                LongUrl = longUrl
            };

            using var conn = new SqlConnection(_connectionString);

            const string sql = @"
            INSERT INTO Urls (Id, Code, LongUrl, CreatedAt, IsActive)
            VALUES (@Id, @Code, @LongUrl, @CreatedAt, @IsActive);
        ";

            conn.Execute(sql, url);

            return url;
        }

        public Url? GetByCode(string code)
        {
            using var conn = new SqlConnection(_connectionString);

            const string sql = @"
            SELECT Id, Code, LongUrl, CreatedAt, IsActive
            FROM Urls
            WHERE Code = @Code AND IsActive = 1;
        ";

            return conn.QueryFirstOrDefault<Url>(sql, new { Code = code });
        }

        private string GenerateCode()
        {
            return Guid.NewGuid().ToString("N")[..8];
        }
    }
}
