using System;
using System.ComponentModel;
using FluentValidation;
using MassTransit;

namespace Reservation.Application.Reservation.Commands.CreateReservation
{
    public class SubmitReservationCommand
    {
        public Guid UserId { get; set; }
        public Guid ResourceId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    
    public class SubmitReservationCommandValidator : AbstractValidator<SubmitReservationCommand>
    {
        public SubmitReservationCommandValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty();
            RuleFor(v => v.ResourceId)
                .NotEmpty();
            RuleFor(v => v.From)
                .NotEmpty();
            RuleFor(v => v.To)
                .NotEmpty();
        }
    }
}