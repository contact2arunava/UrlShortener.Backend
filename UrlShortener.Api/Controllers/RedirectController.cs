using Microsoft.AspNetCore.Mvc;
using UrlShortener.Service;

namespace UrlShortener.Api.Controllers;

[ApiController]
public class RedirectController : ControllerBase
{
    private readonly IUrlService _urlService;
    private readonly IAnalyticsService _analytics;

    public RedirectController(IUrlService urlService, IAnalyticsService analytics)
    {
        _urlService = urlService;
        _analytics = analytics;
    }

    [HttpGet("{code}")]
    public IActionResult RedirectToLongUrl(string code)
    {
        var url = _urlService.GetByCode(code);

        if (url is null || !url.IsActive)
            return NotFound();

        var userAgent = Request.Headers["User-Agent"].ToString();
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var ipHash = ip is null ? null : Convert.ToBase64String(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(ip)
            )
        );

        _analytics.LogHit(url.Id, userAgent, ipHash);

        return Redirect(url.LongUrl);
    }
}
