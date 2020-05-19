using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SignalR.Application.Common.Interfaces;

namespace SignalR.Application.Resource.IntegrationEvents
{
    public class ResourceUpdatedConsumer : IConsumer<ToolBox.Contracts.Resource.ResourceUpdated>
    {
        private readonly IResourceService _resourceService;
        private readonly ILogger<ResourceUpdatedConsumer> _logger;

        public ResourceUpdatedConsumer(IResourceService resourceService, ILogger<ResourceUpdatedConsumer> logger)
        {
            _resourceService = resourceService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Resource.ResourceUpdated> context)
        {
            _logger.LogInformation("ResourceUpdatedConsumer Called");
            await _resourceService.SendResource(new Domain.Entities.Resource
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