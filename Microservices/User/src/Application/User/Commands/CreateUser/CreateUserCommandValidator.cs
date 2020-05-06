using FluentValidation;

namespace User.Application.User.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(v => v.FirstName)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(v => v.LastName)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(v => v.Username)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(v => v.Email)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}