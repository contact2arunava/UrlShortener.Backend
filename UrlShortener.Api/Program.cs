using UrlShortener.Api.Data;
using UrlShortener.Domain;
using Microsoft.Data.SqlClient;
using Swashbuckle.AspNetCore.SwaggerUI; // required for UseSwaggerUI extension
using Swashbuckle.AspNetCore.SwaggerGen; // optional; package must be installed

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // requires Swashbuckle.AspNetCore package

// Dapper repository
builder.Services.AddScoped<IUrlRepository, DapperUrlRepository>();

var app = builder.Build();

app.UseSwagger();       // enable swagger JSON endpoint
app.UseSwaggerUI();     // enable swagger UI

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
