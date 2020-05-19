using System;
using Automatonymous;
using MassTransit;
using ToolBox.Contracts.Reservation;
using ToolBox.Contracts.Resource;

namespace Reservation.Application.Reservation.Commands.CreateReservation
{
    public class CreateReservationState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public SubmitReservation ReservationToCreate { get; set; }
    }
    
    public class CreateReservationStateMachine : MassTransitStateMachine<CreateReservationState>
    {
        public CreateReservationStateMachine()
        {
            InstanceState(x => x.CurrentState, Submitted, Pending);

            Event(() => SubmitReservation, x => x.CorrelateById(context => context.Message.Id));
            Event(() => ResourceReservationCreated, x => x.CorrelateById(context => context.Message.Id));
            Event(() => ReservationCreated, x => x.CorrelateById(context => context.Message.Id));


            Initially(
                When(SubmitReservation)
                    .RespondAsync(x => x.Init<SubmitReservationAccepted>(new
                    {
                        x.Data.Id
                    }))
                    .Then(x => x.Instance.ReservationToCreate = x.Data)
                    .SendAsync(new Uri("queue:create-resource-reservation"), x => x.Init<CreateResourceReservation>(new
                    {
                        x.Instance.ReservationToCreate.Id,
                        x.Instance.ReservationToCreate.UserId,
                        x.Instance.ReservationToCreate.ResourceId,
                        x.Instance.ReservationToCreate.From,
                        x.Instance.ReservationToCreate.To
                    }))
                    .TransitionTo(Submitted));

            During(Submitted,
                When(ResourceReservationCreated)
                    .SendAsync(new Uri("queue:create-reservation"), x => x.Init<ToolBox.Contracts.Reservation.CreateReservation>(new
                    {

                        x.Instance.ReservationToCreate.Id,
                        x.Instance.ReservationToCreate.UserId,
                        x.Instance.ReservationToCreate.ResourceId,
                        x.Instance.ReservationToCreate.From,
                        x.Instance.ReservationToCreate.To
                    }))
                    .TransitionTo(Pending));
            
            During(Pending,
                When(ReservationCreated)
                    .Then(x => Console.WriteLine($"Reservation with Id: {x.Data.Id} Created"))
                    .Finalize());
            
            SetCompletedWhenFinalized();
        }

        public State Submitted { get; set; }
        public State Pending { get; set; }

        public Event<SubmitReservation> SubmitReservation { get; private set; }
        public Event<ResourceReservationCreated> ResourceReservationCreated { get; private set; }
        public Event<ReservationCreated> ReservationCreated { get; private set; }
    }
}