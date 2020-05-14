using System;
using System.Collections.Generic;
using System.ComponentModel;
using FluentValidation;
using NodaTime;
using Resource.Domain.Entities;

namespace Contracts.Resource
{
    public interface SubmitResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DayAndTime> Available { get; set; }
    }

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