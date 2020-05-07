using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Common.Interfaces;
using Auth.Application.User.IntegrationEvents.UserUpdated;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToolBox.Bus.Interfaces;
using ToolBox.Events;

namespace Auth.Application.User.IntegrationEvents.UserDeleted
{
    public class UserDeletedEvent : BaseEvent
    {
        public Guid Id { get; set; }
    }

    public class UserDeletedEventHandler : IEventHandler<UserDeletedEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<UserDeletedEventHandler> _logger;

        public UserDeletedEventHandler(IApplicationDbContext dbContext,
            ILogger<UserDeletedEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Handle(UserDeletedEvent @event)
        {
            _logger.LogInformation("UserDeletedEventHandler Called");
            var userToDelete = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == @event.Id);
            _dbContext.Users.Remove(userToDelete);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            await Task.CompletedTask;
            
        }
    }
}