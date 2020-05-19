using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Resource.Application.Common.Interfaces;
using Resource.Domain.Entities;
using ToolBox.Contracts.Resource;

namespace Resource.Application.Resource.Commands.UpdateResource
{
    public class UpdateResourceConsumer : IConsumer<ToolBox.Contracts.Resource.UpdateResource>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<UpdateResourceConsumer> _logger;

        public UpdateResourceConsumer(IApplicationDbContext dbContext, ILogger<UpdateResourceConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Resource.UpdateResource> context)
        {
            _logger.LogInformation("UpdateResourceConsumer Called");
            
            var entity = await _dbContext.Resources.Include(r => r.Available).FirstOrDefaultAsync(r => r.Id == context.Message.Id);

            if (context.Message.Name != null) entity.Name = context.Message.Name;
            if (context.Message.Description != null) entity.Description = context.Message.Description;
            if (context.Message.Available.Count > 0)
            {
                foreach (var dayAndTime in context.Message.Available)
                {
                    var existingTime = await _dbContext.DayAndTimes.FindAsync(dayAndTime.Id);
                    if (existingTime == null)
                    {
                        entity.Available.Add(dayAndTime);
                        await _dbContext.DayAndTimes.AddAsync(dayAndTime);
                    }
                    else
                    {
                        _dbContext.Entry(existingTime).CurrentValues.SetValues(dayAndTime);
                    }
                }
            }
            else
            {
                entity.Available.Clear();
            }

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            
            await context.Publish<ResourceUpdated>(new
            {
                context.Message.Id,
                context.Message.Name,
                context.Message.Description,
                context.Message.Available
            });
        }
    }
}