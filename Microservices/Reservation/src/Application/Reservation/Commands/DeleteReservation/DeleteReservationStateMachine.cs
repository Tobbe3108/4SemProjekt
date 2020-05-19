using System;
using Automatonymous;
using MassTransit;
using ToolBox.Contracts.Reservation;
using ToolBox.Contracts.Resource;

namespace Reservation.Application.Reservation.Commands.UpdateReservation
{
    public class DeleteReservationState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public SubmitDeleteReservation ReservationToDelete { get; set; }
    }
    
    public class DeleteReservationStateMachine : MassTransitStateMachine<DeleteReservationState>
    {
        public DeleteReservationStateMachine()
        {
            InstanceState(x => x.CurrentState, Submitted, Pending);

            Event(() => SubmitDeleteReservation, x => x.CorrelateById(context => context.Message.Id));
            Event(() => ResourceReservationDeleted, x => x.CorrelateById(context => context.Message.Id));
            Event(() => ReservationDeleted, x => x.CorrelateById(context => context.Message.Id));


            Initially(
                When(SubmitDeleteReservation)
                    .Then(x => x.Instance.ReservationToDelete = x.Data)
                    .SendAsync(new Uri("queue:delete-resource-reservation"),
                        x => x.Init<DeleteResourceReservation>(new
                        {
                            x.Instance.ReservationToDelete.Id,
                        }))
                    .TransitionTo(Submitted));

            During(Submitted,
                When(ResourceReservationDeleted)
                    .SendAsync(new Uri("queue:delete-reservation"),
                        x => x.Init<ToolBox.Contracts.Reservation.DeleteReservation>(new
                        {

                            x.Instance.ReservationToDelete.Id,
                        }))
                    .TransitionTo(Pending));
            
            During(Pending,
                When(ReservationDeleted)
                    .Then(x => Console.WriteLine($"Reservation with Id: {x.Data.Id} deleted"))
                    .Finalize());
            
            SetCompletedWhenFinalized();
        }

        public State Submitted { get; set; }
        public State Pending { get; set; }

        public Event<SubmitDeleteReservation> SubmitDeleteReservation { get; private set; }
        public Event<ResourceReservationDeleted> ResourceReservationDeleted { get; private set; }
        public Event<ReservationDeleted> ReservationDeleted { get; private set; }
    }
}