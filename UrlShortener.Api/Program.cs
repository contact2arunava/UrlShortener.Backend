using UrlShortener.Api.Data;
using UrlShortener.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Dapper repository
builder.Services.AddScoped<IUrlRepository, DapperUrlRepository>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "UrlShortener Backend" }));

app.MapControllers();

app.MapGet("/{code}", (string code, IUrlRepository repo) =>
{
    var url = repo.GetByCode(code);

    if (url is null || !url.IsActive)
        return Results.NotFound();

    return Results.Redirect(url.LongUrl);
});

app.Run();
