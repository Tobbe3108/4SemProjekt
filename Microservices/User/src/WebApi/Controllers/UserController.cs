using System;
using System.ComponentModel;
using System.Threading.Tasks;
using FluentValidation.Results;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToolBox.Contracts;
using ToolBox.Contracts.User;
using User.Application.User.Commands.CreateUser;
using User.Application.User.Commands.DeleteUser;
using User.Application.User.Commands.UpdateUser;
using User.Application.User.Queries.GetUser;

namespace WebApi.Controllers
{
    public class UserController : ApiController
    {
        private readonly IRequestClient<SubmitUser> _submitUserRequestClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public UserController(IRequestClient<SubmitUser> submitUserRequestClient, ISendEndpointProvider sendEndpointProvider)
        {
            _submitUserRequestClient = submitUserRequestClient;
            _sendEndpointProvider = sendEndpointProvider;
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(SubmitUserCommand command)
        {
            //return await Mediator.Send(command);
            
            var validator = new SubmitUserCommandValidator();
            ValidationResult result = await validator.ValidateAsync(command);
            if (!result.IsValid) return BadRequest(result.Errors);
            
            // var (accepted, rejected) = await _submitUserRequestClient.GetResponse<CreateUserAccepted, CreateUserRejected>(new
            // {
            //     command.Id,
            //     command.Username,
            //     command.Email,
            //     command.FirstName,
            //     command.LastName,
            //     command.Password
            // });
            //
            // if (!accepted.IsCompletedSuccessfully)
            // {
            //     return Problem(rejected.Result.Message.Reason);
            // }
            // return Ok(accepted.Result.Message.Id);
            
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:submit-user"));

            await endpoint.Send<SubmitUser>(new
            {

                command.Id,
                command.Username,
                command.Email,
                command.FirstName,
                command.LastName,
                command.Password
            });
            return Ok(command.Id);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserVm>> Get()
        {
            return await Mediator.Send(new GetCurrentUserQuery());
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserVm>> GetById(Guid id)
        {
            return await Mediator.Send(new GetUserQuery { Id = id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            
            await Mediator.Send(command);
            
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteUserCommand {Id = id});

            return NoContent();
        }
    }
}