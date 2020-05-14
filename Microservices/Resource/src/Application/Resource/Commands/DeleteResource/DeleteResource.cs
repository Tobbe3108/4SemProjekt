using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Resource.Application.Common.Interfaces;

namespace Contracts.Resource
{
    public interface DeleteResource
    {
        public Guid Id { get; set; }
    }

    public class DeleteResourceConsumer : IConsumer<DeleteResource>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<DeleteResourceConsumer> _logger;

        public DeleteResourceConsumer(IApplicationDbContext dbContext, ILogger<DeleteResourceConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<DeleteResource> context)
        {
            _logger.LogInformation("DeleteResourceConsumer Called");
            
            var entity = await _dbContext.Resources.FindAsync(context.Message.Id);
            
            _dbContext.Resources.Remove(entity);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            
            await context.Publish<ResourceDeleted>(new
            {
                context.Message.Id
            });
        }
    }
}