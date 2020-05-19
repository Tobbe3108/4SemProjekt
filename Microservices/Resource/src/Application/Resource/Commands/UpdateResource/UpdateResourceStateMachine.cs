using System;
using Automatonymous;
using MassTransit;
using ToolBox.Contracts.Resource;

namespace Resource.Application.Resource.Commands.UpdateResource
{
    public class UpdateResourceState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public SubmitUpdateResource ResourceToUpdate { get; set; }

    }
    public class UpdateResourceStateMachine : MassTransitStateMachine<UpdateResourceState>
    {
        public UpdateResourceStateMachine()
        {
            InstanceState(x => x.CurrentState, Submitted);

            Event(() => SubmitUpdateResource, x => x.CorrelateById(context => context.Message.Id));
            Event(() => ResourceUpdated, x => x.CorrelateById(context => context.Message.Id));


            Initially(
                When(SubmitUpdateResource)
                    .Then(x => x.Instance.ResourceToUpdate = x.Data)
                    .SendAsync(new Uri("queue:update-resource"), x => x.Init<ToolBox.Contracts.Resource.UpdateResource>(new
                    {
                        x.Instance.ResourceToUpdate.Id,
                        x.Instance.ResourceToUpdate.Name,
                        x.Instance.ResourceToUpdate.Description,
                        x.Instance.ResourceToUpdate.Available
                    }))
                    .TransitionTo(Submitted));
            
            During(Submitted,
                When(ResourceUpdated)
                    .Then(x => Console.WriteLine($"Resource with Id: {x.Data.Id} Update"))
                    .Finalize());
            
            SetCompletedWhenFinalized();
        }   
        public State Submitted { get; set; }
        public Event<SubmitUpdateResource> SubmitUpdateResource { get; private set; }
        public Event<ResourceUpdated> ResourceUpdated { get; private set; }

    }
}