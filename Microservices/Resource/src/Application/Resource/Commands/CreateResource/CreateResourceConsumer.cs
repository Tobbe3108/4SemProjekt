using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Resource.Application.Common.Interfaces;
using ToolBox.Contracts.Resource;

namespace Resource.Application.Resource.Commands.CreateResource
{
    public class CreateResourceConsumer : IConsumer<ToolBox.Contracts.Resource.CreateResource>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<CreateResourceConsumer> _logger;

        public CreateResourceConsumer(IApplicationDbContext dbContext, ILogger<CreateResourceConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Resource.CreateResource> context)
        {
            _logger.LogInformation("CreateResourceConsumer Called");

            var entity = new global::Resource.Domain.Entities.Resource
            {
                Id = context.Message.Id,
                Name = context.Message.Name,
                Description = context.Message.Description,
                Available = context.Message.Available
            };
            
            await _dbContext.Resources.AddAsync(entity);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            await context.Publish<ResourceCreated>(new
            {
                entity.Id,
                entity.Name,
                entity.Description,
                entity.Available
            });
        }
    }
}