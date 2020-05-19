using System;
using Automatonymous;
using MassTransit;
using ToolBox.Contracts.Reservation;
using ToolBox.Contracts.Resource;

namespace Reservation.Application.Reservation.Commands.DeleteReservation
{
    public class UpdateReservationState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public SubmitUpdateReservation ReservationToUpdate { get; set; }
    }
    
    public class UpdateReservationStateMachine : MassTransitStateMachine<UpdateReservationState>
    {
        public UpdateReservationStateMachine()
        {
            InstanceState(x => x.CurrentState, Submitted, Pending);

            Event(() => SubmitUpdateReservation, x => x.CorrelateById(context => context.Message.Id));
            Event(() => ResourceReservationUpdated, x => x.CorrelateById(context => context.Message.Id));
            Event(() => ReservationUpdated, x => x.CorrelateById(context => context.Message.Id));


            Initially(
                When(SubmitUpdateReservation)
                    .Then(x => x.Instance.ReservationToUpdate = x.Data)
                    .SendAsync(new Uri("queue:update-resource-reservation"),
                        x => x.Init<UpdateResourceReservation>(new
                        {
                            x.Instance.ReservationToUpdate.Id,
                            x.Instance.ReservationToUpdate.From,
                            x.Instance.ReservationToUpdate.To
                        }))
                    .TransitionTo(Submitted));

            During(Submitted,
                When(ResourceReservationUpdated)
                    .SendAsync(new Uri("queue:update-reservation"),
                        x => x.Init<ToolBox.Contracts.Reservation.UpdateReservation>(new
                        {

                            x.Instance.ReservationToUpdate.Id,
                            x.Instance.ReservationToUpdate.From,
                            x.Instance.ReservationToUpdate.To
                        }))
                    .TransitionTo(Pending));
            
            During(Pending,
                When(ReservationUpdated)
                    .Then(x => Console.WriteLine($"Reservation with Id: {x.Data.Id} updated"))
                    .Finalize());
            
            SetCompletedWhenFinalized();
        }

        public State Submitted { get; set; }
        public State Pending { get; set; }

        public Event<SubmitUpdateReservation> SubmitUpdateReservation { get; private set; }
        public Event<ResourceReservationUpdated> ResourceReservationUpdated { get; private set; }
        public Event<ReservationUpdated> ReservationUpdated { get; private set; }
    }
}