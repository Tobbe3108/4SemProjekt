using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToolBox.Bus.Interfaces;
using ToolBox.Events;

namespace Auth.Application.User.IntegrationEvents.UserUpdated
{
    public class UserUpdatedEvent : BaseEvent
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserUpdatedEventHandler : IEventHandler<UserUpdatedEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IHashService _hashService;
        private readonly ILogger<UserUpdatedEventHandler> _logger;

        public UserUpdatedEventHandler(IApplicationDbContext dbContext, IHashService hashService,
            ILogger<UserUpdatedEventHandler> logger)
        {
            _dbContext = dbContext;
            _hashService = hashService;
            _logger = logger;
        }

        public async Task Handle(UserUpdatedEvent @event)
        {
            _logger.LogInformation("UserUpdatedEventHandler Called");
            var userToUpdate = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == @event.Id);
            var salt = _hashService.GenerateSalt();

            userToUpdate.UserName = @event.Username;
            userToUpdate.Email = @event.Email;
            userToUpdate.PasswordSalt = salt;
            userToUpdate.PasswordHash = _hashService.GenerateHash(@event.Password, salt);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            await Task.CompletedTask;
        }
    }
}