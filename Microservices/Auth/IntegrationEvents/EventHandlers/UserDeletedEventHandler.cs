// using System;
// using System.Threading.Tasks;
// using Auth.Api.Infrastructure;
// using Auth.Api.IntegrationEvents.Events;
// using Auth.Api.Models;
// using RabbitMQ.Bus.Bus.Interfaces;
// using Auth.Api.Infrastructure.Services;
// using Auth.Api.Models.DTO;
//
// namespace Auth.Api.IntegrationEvents.EventHandlers
// {
//     public class UserDeletedEventHandler : IEventHandler<UserDeletedEvent>
//     {
//         private readonly AuthRepository _authRepository;
//
//         public UserDeletedEventHandler(AuthRepository authRepository)
//         {
//             _authRepository = authRepository;
//         }
//
//         async Task IEventHandler<UserDeletedEvent>.Handle(UserDeletedEvent @event)
//         {
//             AuthUser user = await _authRepository.GetUserFromId(@event.User.Id);
//
//             await _authRepository.Delete(user);
//         }
//     }
// }
