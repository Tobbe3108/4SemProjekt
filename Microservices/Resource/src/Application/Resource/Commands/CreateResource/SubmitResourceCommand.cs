using System.Collections.Generic;
using System.ComponentModel;
using FluentValidation;
using Resource.Domain.Entities;
using ToolBox.Contracts.Resource;

namespace Resource.Application.Resource.Commands.CreateResource
{
    public class SubmitResourceCommand
    {
        [DefaultValue("Rom")] public string Name { get; set; }
        [DefaultValue("Mødelokale Rom")] public string Description { get; set; }
        [DefaultValue(default(List<DayAndTime>))] public List<DayAndTime> Available { get; set; }
    }
    
    public class SubmitResourceCommandValidator : AbstractValidator<SubmitResourceCommand>
    {
        public SubmitResourceCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(v => v.Description)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}