using FluentValidation;

namespace User.Application.User.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(v => v.Username)
                .MaximumLength(200);
            RuleFor(v => v.Email)
                .MaximumLength(200);
            RuleFor(v => v.FirstName)
                .MaximumLength(200);
            RuleFor(v => v.LastName)
                .MaximumLength(200);
            RuleFor(v => v.Address)
                .MaximumLength(200);
            RuleFor(v => v.City)
                .MaximumLength(200);
            RuleFor(v => v.Country)
                .MaximumLength(200);
            RuleFor(v => v.ZipCode)
                .GreaterThanOrEqualTo(1000);
        }
    }
}