using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
//using UrlShortener.Api.Controllers;
using Xunit;

public class UrlShortenerApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UrlShortenerApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostUrl_ShouldCreateShortUrl()
    {
        var response = await _client.PostAsJsonAsync("/api/urls", new { longUrl = "https://microsoft.com" });

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);


        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
