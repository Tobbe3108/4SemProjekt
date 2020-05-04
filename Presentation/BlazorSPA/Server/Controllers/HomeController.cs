using Microsoft.AspNetCore.Mvc;

namespace BlazorSPA.Server.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Hello");
        }
    }
}