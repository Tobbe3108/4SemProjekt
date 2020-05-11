using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Logging;
using ToolBox.Contracts.User;
using User.Application.Common.Interfaces;

namespace User.Application.User.Commands.CreateUser
{
    public class CreateUserConsumer : IConsumer<ToolBox.Contracts.User.CreateUser>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<CreateUserConsumer> _logger;

        public CreateUserConsumer(IApplicationDbContext dbContext, ILogger<CreateUserConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.User.CreateUser> context)
        {
            _logger.LogInformation("CreateUserConsumer Called");
            try
            {
                var entity = new Domain.Entities.User
                {
                    Id = context.Message.Id,
                    Username = context.Message.Username,
                    Email = context.Message.Email,
                    FirstName = context.Message.FirstName,
                    LastName = context.Message.LastName,
                    NormalizedUserName = context.Message.Username.ToUpperInvariant(),
                    Password = context.Message.Password,
                };
                
                await _dbContext.Users.AddAsync(entity);

                await _dbContext.SaveChangesAsync(CancellationToken.None);

                await context.Publish<UserCreated>(new
                {
                    Id = entity.Id,
                    Username = entity.Username,
                    Email = entity.Email,
                    Password = entity.Password
                });
                
                await context.RespondAsync<CreateUserAccepted>(new
                {
                    Id = entity.Id
                });
            }
            catch (Exception e)
            {
                await context.RespondAsync<CreateUserRejected>(new
                {
                    Reason = e.Message
                });
            }
        }
    }
}