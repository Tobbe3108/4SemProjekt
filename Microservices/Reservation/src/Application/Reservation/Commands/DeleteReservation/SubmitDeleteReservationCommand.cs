using System;
using FluentValidation;

namespace Reservation.Application.Reservation.Commands.DeleteReservation
{
    public class SubmitDeleteReservationCommand
    {
        public Guid Id { get; set; }
    }
    
    public class SubmitDeleteReservationCommandValidator : AbstractValidator<SubmitDeleteReservationCommand>
    {
        public SubmitDeleteReservationCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty();
        }
    }
}