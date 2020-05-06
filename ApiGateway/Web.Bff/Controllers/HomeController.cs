using Microsoft.AspNetCore.Mvc;

namespace Web.Bff.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Backend for frontend for BlazorSPA");
        }
    }
}