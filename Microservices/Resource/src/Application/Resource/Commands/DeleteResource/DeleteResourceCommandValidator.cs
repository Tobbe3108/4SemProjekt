using FluentValidation;
using Resource.Application.Resource.Commands.UpdateResource;

namespace Resource.Application.Resource.Commands.DeleteResource
{
    public class DeleteResourceCommandValidator : AbstractValidator<DeleteResourceCommand>
    {
        public DeleteResourceCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty();
        }
    }
}