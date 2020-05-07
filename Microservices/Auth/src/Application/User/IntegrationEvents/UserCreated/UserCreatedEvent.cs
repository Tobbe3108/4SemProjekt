using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using ToolBox.Bus.Interfaces;
using ToolBox.Events;

namespace Auth.Application.User.IntegrationEvents.UserCreated
{
    public class UserCreatedEvent : BaseEvent
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IHashService _hashService;
        private readonly ILogger<UserCreatedEventHandler> _logger;

        public UserCreatedEventHandler(IApplicationDbContext dbContext, IHashService hashService, ILogger<UserCreatedEventHandler> logger)
        {
            _dbContext = dbContext;
            _hashService = hashService;
            _logger = logger;
        }

        public async Task Handle(UserCreatedEvent @event)
        {
            _logger.LogInformation("UserCreatedEventHandler Called");
            string salt = _hashService.GenerateSalt();
            Domain.Entities.User userToCreate = new Domain.Entities.User
            {
                Id = @event.Id,
                UserName = @event.Username,
                Email = @event.Email,
                PasswordSalt = salt,
                PasswordHash = _hashService.GenerateHash(@event.Password, salt)
            };

            await _dbContext.Users.AddAsync(userToCreate);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            await Task.CompletedTask;
        }
    }
}