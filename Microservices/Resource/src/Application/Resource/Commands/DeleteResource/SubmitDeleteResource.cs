using System;
using System.Reflection.PortableExecutable;
using FluentValidation;

namespace Contracts.Resource
{
    public interface SubmitDeleteResource
    {
        public Guid Id { get; set; }
    }

    public class SubmitDeleteResourceCommand
    {
        public Guid Id { get; set; }
    }

    public class SubmitDeleteResourceCommandValidator : AbstractValidator<SubmitDeleteResourceCommand>
    {
        public SubmitDeleteResourceCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty();
        }
    }
}