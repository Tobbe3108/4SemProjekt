using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.Api.Infrastructure.Services;
using Auth.Api.Models;
using Auth.Api.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<string>> LoginAsync([FromBody] LoginDTO login)
        {
            if (login == null) return BadRequest();

            try
            {
                var userToken = await _authService.AuthenticateAsync(login);

                if (userToken != null)
                {
                    return Ok("Bearer" + " " + userToken);
                }

                return Unauthorized();
            }
            catch (Exception e)
            {
                return StatusCode(503, e.Message); // Returns Service Unavailable
            }
        }
    }
}