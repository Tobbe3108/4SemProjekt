using System;
using Automatonymous;
using Contracts.AuthUser;
using Contracts.User;
using MassTransit;

namespace User.Application.User.Commands.DeleteUser
{
    public class DeleteUserState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public Guid Id { get; set; }
    }
    
    public class DeleteUserStateMachine : MassTransitStateMachine<DeleteUserState>
    {
        public DeleteUserStateMachine()
        {
            InstanceState(x => x.CurrentState, Submitted, Pending);

            Event(() => SubmitDeleteUser, x => x.CorrelateById(context => context.Message.Id));
            Event(() => AuthUserDeleted, x => x.CorrelateById(context => context.Message.Id));
            Event(() => UserDeleted, x => x.CorrelateById(context => context.Message.Id));


            Initially(
                When(SubmitDeleteUser)
                    .Then(x => x.Instance.Id = x.Data.Id)
                    .SendAsync(new Uri("queue:delete-auth-user"), x =>
                        x.Init<DeleteAuthUser>(new
                        {
                            x.Instance.Id
                        }))
                    .TransitionTo(Submitted));

            During(Submitted,
                When(AuthUserDeleted)
                    .SendAsync(new Uri("queue:delete-user"), x => x.Init<Contracts.User.DeleteUser>(new
                    {
                        x.Instance.Id
                    }))
                    .TransitionTo(Pending));

            During(Pending,
                When(UserDeleted)
                    .Then(x => Console.WriteLine($"User with Id: {x.Data.Id} Deleted"))
                    .Finalize());
            
            SetCompletedWhenFinalized();
        }
        
        public State Submitted { get; set; }
        public State Pending { get; set; }

        public Event<SubmitDeleteUser> SubmitDeleteUser { get; private set; }
        public Event<AuthUserDeleted> AuthUserDeleted { get; private set; }
        public Event<UserDeleted> UserDeleted { get; private set; }
    }
}