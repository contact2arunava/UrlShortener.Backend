using Microsoft.AspNetCore.Mvc;
using UrlShortener.Service;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/urls")]
public class AnalyticsController : ControllerBase
{
    private IAnalyticsService _analyticsService;
    private IUrlService _urlService;

    public AnalyticsController(IAnalyticsService analyticsService, IUrlService urlService)
    {
        _analyticsService = analyticsService;
        _urlService = urlService;
    }

    [HttpGet("{id}/stats")]
    public IActionResult GetStats(string id)
    {
        var url = _urlService.GetByCode(id);

        if (url is null)
            return NotFound();

        var hits = _analyticsService.GetHits(url.Id);

        return Ok(new
        {
            url.Id,
            url.Code,
            url.LongUrl,
            totalHits = hits.Count(),
            lastAccessed = hits.FirstOrDefault()?.Timestamp,
            hits
        });
    }
}
