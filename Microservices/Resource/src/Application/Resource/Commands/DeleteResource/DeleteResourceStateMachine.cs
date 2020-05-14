using System;
using Automatonymous;
using Contracts.Resource;
using MassTransit;

namespace Resource.Application.Resource.Commands.DeleteResource
{
    public class DeleteResourceState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public Guid Id { get; set; }
    }
    
    public class DeleteResourceStateMachine : MassTransitStateMachine<DeleteResourceState>
    {
        public DeleteResourceStateMachine()
        {
            InstanceState(x => x.CurrentState, Submitted);

            Event(() => SubmitDeleteResource, x => x.CorrelateById(context => context.Message.Id));
            Event(() => ResourceDeleted, x => x.CorrelateById(context => context.Message.Id));


            Initially(
                When(SubmitDeleteResource)
                    .Then(x => x.Instance.Id = x.Data.Id)
                    .SendAsync(new Uri("queue:delete-resource"), x => x.Init<Contracts.Resource.DeleteResource>(new
                    {
                        x.Instance.Id
                    }))
                    .TransitionTo(Submitted));
            
            During(Submitted,
                When(ResourceDeleted)
                    .Then(x => Console.WriteLine($"Resource with Id: {x.Data.Id} Deleted"))
                    .Finalize());
            
            SetCompletedWhenFinalized();
        }
        public State Submitted { get; set; }

        public Event<SubmitDeleteResource> SubmitDeleteResource { get; private set; }
        public Event<ResourceDeleted> ResourceDeleted { get; private set; }

    }
}