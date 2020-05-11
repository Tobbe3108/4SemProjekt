using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using User.Application.Common.Interfaces;

namespace Contracts.User
{
    public interface CreateUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
    
    public class CreateUserConsumer : IConsumer<CreateUser>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<CreateUserConsumer> _logger;

        public CreateUserConsumer(IApplicationDbContext dbContext, ILogger<CreateUserConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<CreateUser> context)
        {
            _logger.LogInformation("CreateUserConsumer Called");

            var entity = new global::User.Domain.Entities.User
            {
                Id = context.Message.Id,
                Username = context.Message.Username,
                Email = context.Message.Email,
                FirstName = context.Message.FirstName,
                LastName = context.Message.LastName,
                NormalizedUserName = context.Message.Username.ToUpperInvariant(),
                Password = context.Message.Password
            };
            
            await _dbContext.Users.AddAsync(entity);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            await context.Publish<UserCreated>(new
            {
                entity.Id,
                entity.Username,
                entity.Email,
                entity.Password
            });
        }
    }
}