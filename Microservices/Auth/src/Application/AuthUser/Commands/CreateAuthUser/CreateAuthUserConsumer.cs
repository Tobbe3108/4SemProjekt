using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Common.Interfaces;
using Auth.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToolBox.Contracts.Auth;

namespace Auth.Application.AuthUser.Commands.CreateAuthUser
{
    public class CreateAuthUserConsumer : IConsumer<ToolBox.Contracts.Auth.CreateAuthUser>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IHashService _hashService;
        private readonly ILogger<CreateAuthUserConsumer> _logger;

        public CreateAuthUserConsumer(IApplicationDbContext dbContext, IHashService hashService, ILogger<CreateAuthUserConsumer> logger)
        {
            _dbContext = dbContext;
            _hashService = hashService;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Auth.CreateAuthUser> context)
        { 
            _logger.LogInformation("CreateAuthUserConsumer Called"); 
            string salt = _hashService.GenerateSalt();
            Auth.Domain.Entities.AuthUser authUserToCreate = new Auth.Domain.Entities.AuthUser
            {
               Id = context.Message.Id,
               UserName = context.Message.Username,
               Email = context.Message.Email,
               PasswordSalt = salt,
               PasswordHash = _hashService.GenerateHash(context.Message.Password, salt),
               UserRoles = new List<UserRole>
               {
                   new UserRole
                   {
                       UserId = context.Message.Id,
                       Role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin")
                   }
               }
            };

            await _dbContext.Users.AddAsync(authUserToCreate);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            await context.Publish<AuthUserCreated>(new
            {
               authUserToCreate.Id
            });
        }
    }
}