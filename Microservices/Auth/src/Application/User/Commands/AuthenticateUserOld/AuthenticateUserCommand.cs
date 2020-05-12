
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.User.Commands.AuthenticateUser
{
    public class AuthenticateUserCommand : IRequest<string>
    {
        [DefaultValue("Tobbe3108")] public string UsernameOrEmail { get; set; }
        [DefaultValue("Zxasqw12")] public string Password { get; set; }
    }
    
    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, string>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IHashService _hashService;
        private readonly IAuthService _authService;

        public AuthenticateUserCommandHandler(IApplicationDbContext dbContext, IHashService hashService, IAuthService authService)
        {
            _dbContext = dbContext;
            _hashService = hashService;
            _authService = authService;
        }
        
        public async Task<string> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var userIn = await _dbContext.Users.Include(u => u.UserRoles).ThenInclude(u => u.Role).SingleOrDefaultAsync(u => u.UserName == request.UsernameOrEmail || u.Email == request.UsernameOrEmail);

            if (userIn == null) return null;

            var result = _hashService.Compare(request.Password, userIn.PasswordHash, userIn.PasswordSalt);

            if (result == PasswordVerificationResult.Success)
            {
                return _authService.GenerateJsonWebToken(userIn);
            }

            return null;
        }
    }
}