// using System;
// using System.Threading.Tasks;
// using Auth.Api.Infrastructure;
// using Auth.Api.Infrastructure.Services;
// using Auth.Api.IntegrationEvents.Events;
// using Auth.Api.Models;
// using Auth.Api.Models.DTO;
// using RabbitMQ.Bus.Bus.Interfaces;
//
// namespace Auth.Api.IntegrationEvents.EventHandlers
// {
//     public class UserUpdatedEventHandler : IEventHandler<UserUpdatedEvent>
//     {
//         private readonly HashService _hashService;
//         private readonly AuthRepository _authRepository;
//
//         public UserUpdatedEventHandler(HashService hashService, AuthRepository authRepository)
//         {
//             _hashService = hashService;
//             _authRepository = authRepository;
//         }
//
//         async Task IEventHandler<UserUpdatedEvent>.Handle(UserUpdatedEvent @event)
//         {
//             UpdateUserCredentialsDTO userIn = @event.User;
//             AuthUser userToUpdate = await _authRepository.GetUserFromId(userIn.Id);
//
//             userToUpdate.UserName = userIn.UserName;
//             userToUpdate.Email = userIn.Email;
//
//             userToUpdate.PasswordHash = _hashService.GenerateHash(userToUpdate.PasswordHash, userToUpdate.PasswordSalt);
//
//             await _authRepository.UpdateUser(userToUpdate);
//
//             await Task.CompletedTask;
//         }
//     }
// }