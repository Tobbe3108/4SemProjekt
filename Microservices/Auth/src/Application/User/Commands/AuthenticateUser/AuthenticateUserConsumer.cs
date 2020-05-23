using System.ComponentModel;
using System.Threading.Tasks;
using Auth.Application.Common.Interfaces;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToolBox.Contracts;

namespace Auth.Application.User.Commands.AuthenticateUser
{
    public class AuthenticateUserCommand
    {
        [DefaultValue("Admin")] public string UsernameOrEmail { get; set; }
        [DefaultValue("Zxasqw12")] public string Password { get; set; }
    }

    public class AuthenticateUserCommandValidator :  AbstractValidator<AuthenticateUserCommand>
    {
        public AuthenticateUserCommandValidator()
        {
            RuleFor(v => v.UsernameOrEmail)
                .MaximumLength(200)
                .NotEmpty();
            
            RuleFor(v => v.Password)
                .MaximumLength(200)
                .NotEmpty();
        }
    }

    public class AuthenticateUserConsumer : IConsumer<ToolBox.Contracts.Auth.AuthenticateUser>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IHashService _hashService;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticateUserConsumer> _logger;

        public AuthenticateUserConsumer(IApplicationDbContext dbContext, IHashService hashService, IAuthService authService, ILogger<AuthenticateUserConsumer> logger)
        {
            _dbContext = dbContext;
            _hashService = hashService;
            _authService = authService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Auth.AuthenticateUser> context)
        {
            _logger.LogInformation("AuthenticateUserTestConsumer Called");
            
            var userIn = await _dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .SingleOrDefaultAsync(u => u.UserName == context.Message.UsernameOrEmail 
                                           || u.Email == context.Message.UsernameOrEmail);

            if (userIn == null) await context.RespondAsync<NotFound>(new
            {
                Message = $"User: {context.Message.UsernameOrEmail} was not found"
            });

            var result = _hashService.Compare(context.Message.Password, userIn.PasswordHash, userIn.PasswordSalt);

            if (result == PasswordVerificationResult.Success)
            {
                await context.RespondAsync<Token>(new
                {
                    Token = _authService.GenerateJsonWebToken(userIn)
                });
            }
        }
    }
}