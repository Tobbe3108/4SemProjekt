using System;
using System.Threading.Tasks;
using Contracts.User;
using FluentValidation.Results;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var validator = new SubmitUserCommandValidator();
            ValidationResult result = await validator.ValidateAsync(command);
            if (!result.IsValid) return BadRequest(result.Errors);
            
            var response = await _submitUserRequestClient.GetResponse<SubmitUserAccepted>(new
            {
                Id = Guid.NewGuid(),
                command.Username,
                command.Email,
                command.FirstName,
                command.LastName,
                command.Password
            });
            
            return Ok(response.Message.Id);
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
            var request = new SubmitDeleteUserCommand{ Id = id };
            var validator = new SubmitDeleteUserCommandValidator();
            ValidationResult result = await validator.ValidateAsync(request);
            if (!result.IsValid) return BadRequest(result.Errors);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:submit-delete-user"));
            await endpoint.Send<SubmitDeleteUser>(new
            {
                Id = request.Id
            });

            return NoContent();
        }
    }
}