using System.Threading.Tasks;
using Auth.Application.User.Commands.AuthenticateUser;
using Microsoft.AspNetCore.Mvc;

namespace Auth.WebApi.Controllers
{
    public class AuthController : ApiController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(AuthenticateUserCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}