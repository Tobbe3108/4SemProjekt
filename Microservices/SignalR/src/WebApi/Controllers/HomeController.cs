using Microsoft.AspNetCore.Mvc;

namespace SignalR.WebApi.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("SignalR service");
        }
    }
}