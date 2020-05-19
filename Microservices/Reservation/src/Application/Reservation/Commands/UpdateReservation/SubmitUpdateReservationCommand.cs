using System;
using FluentValidation;
using Reservation.Application.Reservation.Commands.CreateReservation;

namespace Reservation.Application.Reservation.Commands.UpdateReservation
{
    public class SubmitUpdateReservationCommand
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    
    public class SubmitUpdateReservationCommandValidator : AbstractValidator<SubmitUpdateReservationCommand>
    {
        public SubmitUpdateReservationCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty();
            RuleFor(v => v.From)
                .NotEmpty();
            RuleFor(v => v.To)
                .NotEmpty();
        }
    }
}