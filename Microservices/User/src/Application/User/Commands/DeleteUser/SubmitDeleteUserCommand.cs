using System;
using FluentValidation;

namespace User.Application.User.Commands.DeleteUser
{
    public class SubmitDeleteUserCommand
    {
        public Guid Id { get; set; }
    }

    public class SubmitDeleteUserCommandValidator : AbstractValidator<SubmitDeleteUserCommand>
    {
        public SubmitDeleteUserCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty();
        }
    }
}