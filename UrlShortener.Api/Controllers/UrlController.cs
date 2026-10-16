using Microsoft.AspNetCore.Mvc;
using UrlShortener.Domain;

namespace UrlShortener.Api.Controllers
{
    [ApiController]
    [Route("api/urls")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlRepository _repo;

        public UrlController(IUrlRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUrlRequest request)
        {
            if (!Uri.IsWellFormedUriString(request.LongUrl, UriKind.Absolute))
                return BadRequest("Invalid URL");

            var url = _repo.Create(request.LongUrl);

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
