using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Resource.Application.Common.Interfaces;
using ToolBox.Contracts.Resource;

namespace Resource.Application.Resource.Commands.DeleteResource
{
    public class DeleteResourceConsumer : IConsumer<ToolBox.Contracts.Resource.DeleteResource>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<DeleteResourceConsumer> _logger;

        public DeleteResourceConsumer(IApplicationDbContext dbContext, ILogger<DeleteResourceConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Resource.DeleteResource> context)
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