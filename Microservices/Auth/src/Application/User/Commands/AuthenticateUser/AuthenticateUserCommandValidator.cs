using FluentValidation;

namespace Auth.Application.User.Commands.AuthenticateUser
{
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
}