using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Common.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contracts.AuthUser
{
    public interface DeleteAuthUser
    {
        public Guid Id { get; set; }
    }

    public class DeleteAuthUserConsumer : IConsumer<DeleteAuthUser>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<DeleteAuthUserConsumer> _logger;

        public DeleteAuthUserConsumer(IApplicationDbContext dbContext,
            ILogger<DeleteAuthUserConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<DeleteAuthUser> context)
        {
            _logger.LogInformation("DeleteAuthUserConsumer Called");
            var userToDelete = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == context.Message.Id);
            _dbContext.Users.Remove(userToDelete);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            
            await context.Publish<AuthUserDeleted>(new
            {
                userToDelete.Id
            });
        }
    }
}