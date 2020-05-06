using System.Security.Claims;
using Auth.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Auth.WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            Username = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string Username { get; }
    }
}