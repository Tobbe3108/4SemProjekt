using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Resource.Application.Common.Interfaces;
using Resource.Domain.Entities;

namespace Contracts.Resource
{
    public interface UpdateResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DayAndTime> Available { get; set; }
    }

    public class UpdateResourceConsumer : IConsumer<UpdateResource>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<UpdateResourceConsumer> _logger;

        public UpdateResourceConsumer(IApplicationDbContext dbContext, ILogger<UpdateResourceConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<UpdateResource> context)
        {
            _logger.LogInformation("UpdateResourceConsumer Called");
            
            var entity = await _dbContext.Resources.FindAsync(context.Message.Id);

            if (context.Message.Name != null) entity.Name = context.Message.Name;
            if (context.Message.Description != null) entity.Description = context.Message.Description;
            if (context.Message.Available != null) entity.Available = context.Message.Available;

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            
            await context.Publish<ResourceUpdated>(new
            {
                context.Message.Id
            });
        }
    }
}