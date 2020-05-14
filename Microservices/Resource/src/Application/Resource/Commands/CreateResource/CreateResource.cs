using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using NodaTime;
using Resource.Application.Common.Interfaces;
using Resource.Domain.Entities;

namespace Contracts.Resource
{
    public interface CreateResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DayAndTime> Available { get; set; }
    }

    public class CreateResourceConsumer : IConsumer<CreateResource>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<CreateResourceConsumer> _logger;

        public CreateResourceConsumer(IApplicationDbContext dbContext, ILogger<CreateResourceConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<CreateResource> context)
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