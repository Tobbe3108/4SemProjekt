﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalR.Application.Common.Interfaces;
using SignalR.Domain.Entities;
using SignalR.WebApi.Hubs;

namespace SignalR.WebApi.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IHubContext<ResourceHub> _hubContext;

        public ResourceService(IHubContext<ResourceHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendResource(Resource resource)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveResource", resource);
        }
    }
}