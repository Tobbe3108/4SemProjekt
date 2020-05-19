using System.ComponentModel;
using FluentValidation;

namespace User.Application.User.Commands.CreateUser
{
    public class SubmitUserCommand
    {
        [DefaultValue("Trut1936")] public string Username { get; set; }
        [DefaultValue("ThomasFBrandt@rhyta.com")] public string Email { get; set; }
        [DefaultValue("Thomas")] public string FirstName { get; set; }
        [DefaultValue("Brandt")] public string LastName { get; set; }
        [DefaultValue("Zxasqw12")] public string Password { get; set; }
    }
    
    public class SubmitUserCommandValidator : AbstractValidator<SubmitUserCommand>
    {
        public SubmitUserCommandValidator()
        {
            RuleFor(v => v.FirstName)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(v => v.LastName)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(v => v.Username)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(v => v.Email)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}