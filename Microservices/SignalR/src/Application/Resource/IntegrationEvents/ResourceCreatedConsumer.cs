using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SignalR.Application.Common.Interfaces;

namespace SignalR.Application.Resource.IntegrationEvents
{
    public class ResourceCreatedConsumer : IConsumer<ToolBox.Contracts.Resource.ResourceCreated>
    {
        private readonly IResourceService _resourceService;
        private readonly ILogger<ResourceCreatedConsumer> _logger;

        public ResourceCreatedConsumer(IResourceService resourceService, ILogger<ResourceCreatedConsumer> logger)
        {
            _resourceService = resourceService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Resource.ResourceCreated> context)
        {
            _logger.LogInformation("ResourceCreatedConsumer Called");
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