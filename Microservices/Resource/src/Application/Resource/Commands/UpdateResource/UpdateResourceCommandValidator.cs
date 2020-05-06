using FluentValidation;
using Resource.Application.Resource.Commands.CreateResource;

namespace Resource.Application.Resource.Commands.UpdateResource
{
    public class UpdateResourceCommandValidator : AbstractValidator<UpdateResourceCommand>
    {
        public UpdateResourceCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}