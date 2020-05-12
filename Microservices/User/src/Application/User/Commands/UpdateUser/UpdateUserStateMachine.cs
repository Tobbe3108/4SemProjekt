using System;
using Automatonymous;
using Contracts.AuthUser;
using Contracts.User;
using MassTransit;
using User.Application.User.Commands.DeleteUser;

namespace User.Application.User.Commands.UpdateUser
{
    public class UpdateUserState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public SubmitUpdateUser UserToUpdate { get; set; }
    }
    
    public class UpdateUserStateMachine : MassTransitStateMachine<UpdateUserState>
    {
        public UpdateUserStateMachine()
        {
            InstanceState(x => x.CurrentState, Submitted, Pending);

            Event(() => SubmitUpdateUser, x => x.CorrelateById(context => context.Message.Id));
            Event(() => AuthUserUpdated, x => x.CorrelateById(context => context.Message.Id));
            Event(() => UserUpdated, x => x.CorrelateById(context => context.Message.Id));


            Initially(
                When(SubmitUpdateUser)
                    .Then(x => x.Instance.UserToUpdate = x.Data)
                    .SendAsync(new Uri("queue:update-auth-user"), x =>
                        x.Init<UpdateAuthUser>(new
                        {
                            x.Instance.UserToUpdate.Id,
                            x.Instance.UserToUpdate.Username,
                            x.Instance.UserToUpdate.Email,
                            x.Instance.UserToUpdate.Password
                        }))
                    .TransitionTo(Submitted));

            During(Submitted,
                When(AuthUserUpdated)
                    .SendAsync(new Uri("queue:update-user"), x => x.Init<Contracts.User.UpdateUser>(new
                    {
                        x.Instance.UserToUpdate.Id,
                        x.Instance.UserToUpdate.Username,
                        x.Instance.UserToUpdate.Email,
                        x.Instance.UserToUpdate.Password,
                        x.Instance.UserToUpdate.FirstName,
                        x.Instance.UserToUpdate.LastName,
                        x.Instance.UserToUpdate.Address,
                        x.Instance.UserToUpdate.City,
                        x.Instance.UserToUpdate.Country,
                        x.Instance.UserToUpdate.ZipCode
                    }))
                    .TransitionTo(Pending));

            During(Pending,
                When(UserUpdated)
                    .Then(x => Console.WriteLine($"User with Id: {x.Data.Id} Update"))
                    .Finalize());
            
            SetCompletedWhenFinalized();
        }
        
        public State Submitted { get; set; }
        public State Pending { get; set; }

        public Event<SubmitUpdateUser> SubmitUpdateUser { get; private set; }
        public Event<AuthUserUpdated> AuthUserUpdated { get; private set; }
        public Event<UserUpdated> UserUpdated { get; private set; }
    }
}