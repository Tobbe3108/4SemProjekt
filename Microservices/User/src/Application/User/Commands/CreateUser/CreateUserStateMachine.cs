using System;
using Automatonymous;
using GreenPipes.Caching.Internals;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToolBox.Contracts.AuthUser;
using ToolBox.Contracts.User;

namespace User.Application.User.Commands.CreateUser
{
    public class CreateUserState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public ToolBox.Contracts.User.SubmitUser UserToCreate { get; set; }
    }
    
    public class CreateUserStateMap : SagaClassMap<CreateUserState>
    {
        protected override void Configure(EntityTypeBuilder<CreateUserState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(64);
        }
    }

    
    public class CreateUserStateMachine : MassTransitStateMachine<CreateUserState>
    {
        public CreateUserStateMachine()
        {
            InstanceState(x => x.CurrentState, Submitted, Pending, Created);

            Event(() => SubmitUser, x => x.CorrelateById(context => context.Message.Id));
            Event(() => AuthUserCreated, x => x.CorrelateById(context => context.Message.Id));
            Event(() => UserCreated, x => x.CorrelateById(context => context.Message.Id));


            Initially(
                When(SubmitUser)
                    .Then(x => x.Instance.UserToCreate = x.Data)
                    .SendAsync(new Uri("queue:create-auth-user"), context =>
                        context.Init<CreateAuthUser>(new
                        {
                            context.Instance.UserToCreate.Id,
                            context.Instance.UserToCreate.Username,
                            context.Instance.UserToCreate.Email,
                            context.Instance.UserToCreate.Password
                        }))
                    .TransitionTo(Submitted));
                // When(AuthUserCreated)
                //     .Send(new Uri("queue:create-user"), context => context.Init<ToolBox.Contracts.User.CreateUser>(new
                //     {
                //         context.Instance.UserToCreate.Id,
                //         context.Instance.UserToCreate.Username,
                //         context.Instance.UserToCreate.Email,
                //         context.Instance.UserToCreate.FirstName,
                //         context.Instance.UserToCreate.LastName,
                //         context.Instance.UserToCreate.Password
                //     }))
                //     .TransitionTo(Pending),
                // When(UserCreated)
                //     .TransitionTo(Created));
            
            During(Submitted,
                When(AuthUserCreated)
                .Send(new Uri("queue:create-user"), context => context.Init<ToolBox.Contracts.User.CreateUser>(new
                {
                    context.Instance.UserToCreate.Id,
                    context.Instance.UserToCreate.Username,
                    context.Instance.UserToCreate.Email,
                    context.Instance.UserToCreate.FirstName,
                    context.Instance.UserToCreate.LastName,
                    context.Instance.UserToCreate.Password
                }))
                .TransitionTo(Pending));
            
            During(Pending,
                When(UserCreated)
                    .TransitionTo(Created));
        }

        public State Submitted { get; set; }
        public State Pending { get; set; }
        public State Created { get; private set; }
        
        public Event<SubmitUser> SubmitUser { get; private set; }
        public Event<AuthUserCreated> AuthUserCreated { get; private set; }
        public Event<UserCreated> UserCreated { get; private set; }
    }
}