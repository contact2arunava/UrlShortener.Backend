using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Api.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetHealth()
        {
            return Ok(new { status = "ok", service = "UrlShortener Backend" });
        }
    }
}
