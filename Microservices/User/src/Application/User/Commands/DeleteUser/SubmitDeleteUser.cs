using System;
using FluentValidation;

namespace Contracts.User
{
    public interface SubmitDeleteUser
    {
        public Guid Id { get; set; }
    }

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