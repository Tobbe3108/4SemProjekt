using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using User.Application.Common.Interfaces;

namespace Contracts.User
{
    public interface DeleteUser
    {
        public Guid Id { get; set; }
    }

    public class DeleteUserConsumer : IConsumer<DeleteUser>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<DeleteUserConsumer> _logger;

        public DeleteUserConsumer(IApplicationDbContext dbContext, ILogger<DeleteUserConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<DeleteUser> context)
        {
            _logger.LogInformation("DeleteUserConsumer Called");
            
            var entity = await _dbContext.Users.FindAsync(context.Message.Id);
            
            _dbContext.Users.Remove(entity);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            
            await context.Publish<UserDeleted>(new
            {
                context.Message.Id
            });
        }
    }
}