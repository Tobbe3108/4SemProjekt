using System;
using System.Collections.Generic;
using System.ComponentModel;
using FluentValidation;
using Resource.Domain.Entities;

namespace Contracts.Resource
{
    public interface SubmitUpdateResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DayAndTime> Available { get; set; }
    }

    public class SubmitUpdateResourceCommand
    {
        [DefaultValue("")] public Guid Id { get; set; }
        [DefaultValue("Helsinki")] public string Name { get; set; }
        [DefaultValue("Mødelokale Helsinki")] public string Description { get; set; }
        [DefaultValue(default(List<DayAndTime>))] public List<DayAndTime> Available { get; set; }
    }

    public class SubmitUpdateResourceCommandValidator : AbstractValidator<SubmitUpdateResourceCommand>
    {
        public SubmitUpdateResourceCommandValidator()
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