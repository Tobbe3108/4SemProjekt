using System;
using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Common.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contracts.AuthUser
{
    public interface UpdateAuthUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class UpdateAuthUserConsumer : IConsumer<UpdateAuthUser>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IHashService _hashService;
        private readonly ILogger<UpdateAuthUserConsumer> _logger;

        public UpdateAuthUserConsumer(IApplicationDbContext dbContext, IHashService hashService,
            ILogger<UpdateAuthUserConsumer> logger)
        {
            _dbContext = dbContext;
            _hashService = hashService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<Contracts.AuthUser.UpdateAuthUser> context)
        {
            _logger.LogInformation("UpdateAuthUserConsumer Called");
            
            var userToUpdate = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == context.Message.Id);
            var salt = _hashService.GenerateSalt();

            userToUpdate.UserName = context.Message.Username;
            userToUpdate.Email = context.Message.Email;
            userToUpdate.PasswordSalt = salt;
            userToUpdate.PasswordHash = _hashService.GenerateHash(context.Message.Password, salt);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            
            await context.Publish<AuthUserUpdated>(new
            {
                userToUpdate.Id
            });
        }
    }
}