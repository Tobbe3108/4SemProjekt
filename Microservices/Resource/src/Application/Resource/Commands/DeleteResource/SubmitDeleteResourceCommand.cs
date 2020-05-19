using System;
using FluentValidation;

namespace Resource.Application.Resource.Commands.DeleteResource
{
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