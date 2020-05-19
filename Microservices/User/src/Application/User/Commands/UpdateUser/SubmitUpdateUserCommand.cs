using System;
using System.ComponentModel;
using FluentValidation;

namespace User.Application.User.Commands.UpdateUser
{
    public class SubmitUpdateUserCommand
    {
        [DefaultValue("")] public Guid Id { get; set; }
        [DefaultValue("Trut1936")] public string Username { get; set; }
        [DefaultValue("ThomasFBrandt@rhyta.com")] public string Email { get; set; }
        [DefaultValue("Zxasqw12")] public string Password { get; set; }
        [DefaultValue("Thomas")] public string FirstName { get; set; }
        [DefaultValue("Brandt")] public string LastName { get; set; }
        [DefaultValue("Mølleløkken 47")] public string Address { get; set; }
        [DefaultValue("Odder")] public string City { get; set; }
        [DefaultValue("Denmark")] public string Country { get; set; }
        [DefaultValue("9300")] public int? ZipCode { get; set; }
    }

    public class SubmitUpdateUserCommandValidator : AbstractValidator<SubmitUpdateUserCommand>
    {
        public SubmitUpdateUserCommandValidator()
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