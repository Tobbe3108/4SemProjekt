using System;
using System.Threading.Tasks;
using Contracts.AuthUser;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Auth.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRequestClient<AuthenticateUser> _getUserRequestClient;

        public AuthController(IRequestClient<AuthenticateUser> getUserRequestClient)
        {
            _getUserRequestClient = getUserRequestClient;
        }
        
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(AuthenticateUserCommand command)
        {
            var validator = new AuthenticateUserCommandValidator();
            var result = await validator.ValidateAsync(command);
            if (!result.IsValid) return BadRequest(result.Errors);
            
            var (token, notFound) = await _getUserRequestClient.GetResponse<Token, NotFound>(new
            {
                command.UsernameOrEmail,
                command.Password
            });
            
            return token.IsCompletedSuccessfully ? Ok(token.Result.Message.Token) : Problem(notFound.Result.Message.Message);
        }
    }
}