using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SignalR.Application.Common.Interfaces;
using SignalR.Domain.Enums;

namespace SignalR.Application.Resource.IntegrationEvents
{
    public class ResourceDeletedConsumer : IConsumer<ToolBox.Contracts.Resource.ResourceDeleted>
    {
        private readonly IResourceService _resourceService;
        private readonly ILogger<ResourceDeletedConsumer> _logger;

        public ResourceDeletedConsumer(IResourceService resourceService, ILogger<ResourceDeletedConsumer> logger)
        {
            _resourceService = resourceService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Resource.ResourceDeleted> context)
        {
            _logger.LogInformation("ResourceDeletedConsumer Called");
            await _resourceService.SendResource(Type.Delete, new Domain.Entities.Resource
            {
                Id = context.Message.Id,
                Name = context.Message.Name,
                Description = context.Message.Description,
                Available = context.Message.Available
            });
            await Task.CompletedTask;
        }
    }
}