using Microsoft.AspNetCore.Mvc;

namespace Mobile.Bff.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Backend for frontend for XamarinApp");
        }
    }
}