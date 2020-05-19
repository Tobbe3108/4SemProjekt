﻿using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SignalR.Application.Common.Interfaces;

namespace SignalR.WebApi.Services
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