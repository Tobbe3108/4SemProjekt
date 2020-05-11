using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Common.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using ToolBox.Contracts.AuthUser;

namespace Auth.Application.User.Commands.CreateAuthUser
{
    public class CreateAuthUserConsumer : IConsumer<ToolBox.Contracts.AuthUser.CreateAuthUser>
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
        public async Task Consume(ConsumeContext<ToolBox.Contracts.AuthUser.CreateAuthUser> context)
        {
           _logger.LogInformation("CreateAuthUserConsumer Called"); 
           string salt = _hashService.GenerateSalt();
           Domain.Entities.AuthUser authUserToCreate = new Domain.Entities.AuthUser
           {
               Id = context.Message.Id,
               UserName = context.Message.Username,
               Email = context.Message.Email,
               PasswordSalt = salt,
               PasswordHash = _hashService.GenerateHash(context.Message.Password, salt)
           };

           await _dbContext.Users.AddAsync(authUserToCreate);
           await _dbContext.SaveChangesAsync(CancellationToken.None);
           
           await context.Publish<AuthUserCreated>(new
           {
               Id = authUserToCreate.Id
           });
        }
    }
}