using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Common.Interfaces;
using Auth.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contracts.AuthUser
{
    public interface CreateAuthUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class CreateAuthUserConsumer : IConsumer<CreateAuthUser>
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
        public async Task Consume(ConsumeContext<CreateAuthUser> context)
        {
           _logger.LogInformation("CreateAuthUserConsumer Called"); 
           string salt = _hashService.GenerateSalt();
           Auth.Domain.Entities.AuthUser authUserToCreate = new Auth.Domain.Entities.AuthUser
           {
               Id = context.Message.Id,
               UserName = context.Message.Username,
               Email = context.Message.Email,
               PasswordSalt = salt,
               PasswordHash = _hashService.GenerateHash(context.Message.Password, salt)
           };

           await _dbContext.Users.AddAsync(authUserToCreate);

           var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
           await _dbContext.UserRoles.AddAsync(new UserRole {UserId = authUserToCreate.Id, Role = role});
           
           await _dbContext.SaveChangesAsync(CancellationToken.None);
           
           await context.Publish<AuthUserCreated>(new
           {
               authUserToCreate.Id
           });
        }
    }
}