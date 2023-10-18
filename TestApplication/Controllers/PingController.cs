using Microsoft.AspNetCore.Mvc;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetServiceInfo()
        {
            return Ok("Dogshouseservice.Version1.0.1");
        }
    }
}
