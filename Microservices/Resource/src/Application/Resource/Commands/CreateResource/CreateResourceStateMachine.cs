using System;
using Automatonymous;
using MassTransit;
using ToolBox.Contracts.Resource;

namespace Resource.Application.Resource.Commands.CreateResource
{
    public class CreateResourceState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public SubmitResource ResourceToCreate { get; set; }
    }
    
    public class CreateResourceStateMachine : MassTransitStateMachine<CreateResourceState>
    {
        public CreateResourceStateMachine()
        {
            InstanceState(x => x.CurrentState, Submitted);

            Event(() => SubmitResource, x => x.CorrelateById(context => context.Message.Id));
            Event(() => ResourceCreated, x => x.CorrelateById(context => context.Message.Id));
            
            Initially(
                When(SubmitResource)
                    .RespondAsync(x => x.Init<SubmitResourceAccepted>(new
                    {
                        Id = x.Data.Id
                    }))
                    .Then(x => x.Instance.ResourceToCreate = x.Data)
                    .SendAsync(new Uri("queue:create-resource"), x =>
                        x.Init<ToolBox.Contracts.Resource.CreateResource>(new
                        {
                            x.Instance.ResourceToCreate.Id,
                            x.Instance.ResourceToCreate.Name,
                            x.Instance.ResourceToCreate.Description,
                            x.Instance.ResourceToCreate.Available
                        }))
                    .TransitionTo(Submitted));
            
            During(Submitted,
                When(ResourceCreated)
                    .Then(x => Console.WriteLine($"Resource with Id: {x.Data.Id} Created"))
                    .Finalize());
            
            SetCompletedWhenFinalized();
        }
        
        public State Submitted { get; set; }

        public Event<SubmitResource> SubmitResource { get; private set; } 
        public Event<ResourceCreated> ResourceCreated { get; private set; }
    }
}