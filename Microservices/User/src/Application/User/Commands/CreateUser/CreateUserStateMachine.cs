using System;
using Automatonymous;
using MassTransit;
using ToolBox.Contracts.Auth;
using ToolBox.Contracts.User;

namespace User.Application.User.Commands.CreateUser
{
    public class CreateUserState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public SubmitUser UserToCreate { get; set; }
    }
    
    public class CreateUserStateMachine : MassTransitStateMachine<CreateUserState>
    {
        public CreateUserStateMachine()
        {
            InstanceState(x => x.CurrentState, Submitted, Pending);

            Event(() => SubmitUser, x => x.CorrelateById(context => context.Message.Id));
            Event(() => AuthUserCreated, x => x.CorrelateById(context => context.Message.Id));
            Event(() => UserCreated, x => x.CorrelateById(context => context.Message.Id));


            Initially(
                When(SubmitUser)
                    .RespondAsync(x => x.Init<SubmitUserAccepted>(new
                    {
                        Id = x.Data.Id
                    }))
                    .Then(x => x.Instance.UserToCreate = x.Data)
                    .SendAsync(new Uri("queue:create-auth-user"), x =>
                        x.Init<CreateAuthUser>(new
                        {
                            x.Instance.UserToCreate.Id,
                            x.Instance.UserToCreate.Username,
                            x.Instance.UserToCreate.Email,
                            x.Instance.UserToCreate.Password
                        }))
                    .TransitionTo(Submitted));

            During(Submitted,
                When(AuthUserCreated)
                .SendAsync(new Uri("queue:create-user"), x => x.Init<ToolBox.Contracts.User.CreateUser>(new
                {
                    x.Instance.UserToCreate.Id,
                    x.Instance.UserToCreate.Username,
                    x.Instance.UserToCreate.Email,
                    x.Instance.UserToCreate.FirstName,
                    x.Instance.UserToCreate.LastName,
                    x.Instance.UserToCreate.Password
                }))
                .TransitionTo(Pending));

            During(Pending,
                When(UserCreated)
                    .Then(x => Console.WriteLine($"User with Id: {x.Data.Id} Created"))
                    .Finalize());
            
            SetCompletedWhenFinalized();
        }

        public State Submitted { get; set; }
        public State Pending { get; set; }

        public Event<SubmitUser> SubmitUser { get; private set; }
        public Event<AuthUserCreated> AuthUserCreated { get; private set; }
        public Event<UserCreated> UserCreated { get; private set; }
    }
}