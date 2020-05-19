using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Common.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToolBox.Contracts.Auth;

namespace Auth.Application.AuthUser.Commands.DeleteAuthUser
{
    public class DeleteAuthUserConsumer : IConsumer<ToolBox.Contracts.Auth.DeleteAuthUser>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<DeleteAuthUserConsumer> _logger;

        public DeleteAuthUserConsumer(IApplicationDbContext dbContext,
            ILogger<DeleteAuthUserConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Auth.DeleteAuthUser> context)
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