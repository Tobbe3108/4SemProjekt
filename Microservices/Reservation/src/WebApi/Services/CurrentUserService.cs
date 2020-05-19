using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Reservation.Application.Common.Interfaces;

namespace Reservation.WebApi.Services
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