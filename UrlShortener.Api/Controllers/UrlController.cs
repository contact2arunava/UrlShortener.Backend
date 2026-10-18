using Microsoft.AspNetCore.Mvc;
using UrlShortener.Service;

namespace UrlShortener.Api.Controllers
{
    [ApiController]
    [Route("api/urls")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _service;

        public UrlController(IUrlService service)
        {
            _service = service;
        }


        [HttpPost]
        public IActionResult Create([FromBody] CreateUrlRequest request)
        {
            if (!Uri.IsWellFormedUriString(request.LongUrl, UriKind.Absolute))
                return BadRequest("Invalid URL");

            var url = _service.CreateShortUrl(request.LongUrl);

            return Ok(new
            {
                id = url.Id,
                code = url.Code,
                shortUrl = $"{Request.Scheme}://{Request.Host}/{url.Code}",
                longUrl = url.LongUrl
            });
        }
    }

    public class CreateUrlRequest
    {
        public string LongUrl { get; set; } = default!;
    }
}
