using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolBox.Contracts;
using ToolBox.Contracts.User;
using User.Application.Common.Interfaces;
using User.Application.User.Commands.CreateUser;
using User.Application.User.Commands.DeleteUser;
using User.Application.User.Commands.UpdateUser;
using User.Application.User.Queries.GetUser;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IRequestClient<GetUser> _getUserRequestClient;
        private readonly IRequestClient<SubmitUser> _submitUserRequestClient;
        private readonly IRequestClient<GetCurrentUser> _getCurrentUserRequestClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly ICurrentUserService _userService;

        public UserController(IApplicationDbContext dbContext, IRequestClient<GetUser> getUserRequestClient, IRequestClient<SubmitUser> submitUserRequestClient, IRequestClient<GetCurrentUser> getCurrentUserRequestClient , ISendEndpointProvider sendEndpointProvider, ICurrentUserService userService)
        {
            _dbContext = dbContext;
            _getUserRequestClient = getUserRequestClient;
            _submitUserRequestClient = submitUserRequestClient;
            _getCurrentUserRequestClient = getCurrentUserRequestClient;
            _sendEndpointProvider = sendEndpointProvider;
            _userService = userService;
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(SubmitUserCommand command)
        {
            var validator = new SubmitUserCommandValidator();
            ValidationResult result = await validator.ValidateAsync(command);
            if (!result.IsValid) return BadRequest(result.Errors);

            if (_dbContext.Users.Any(u => u.Username == command.Username)) return BadRequest("Username already taken");
            if (_dbContext.Users.Any(u => u.Email == command.Email)) return BadRequest("Email already taken");
            
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
        public async Task<IActionResult> Get()
        {
            var (userVm, notFound) = await _getCurrentUserRequestClient.GetResponse<UserVm, NotFound>(new
            {
                Username = _userService.Username.ToUpperInvariant()
            });
            if (userVm.IsCompletedSuccessfully)
                return new OkObjectResult(userVm.Result.Message);
            else
                return Problem(notFound.Result.Message.Message);
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var (userVm, notFound) = await _getUserRequestClient.GetResponse<UserVm, NotFound>(new
            {
                Id = id
            });
            return userVm.IsCompletedSuccessfully ? Ok(userVm.Result.Message.User) : Problem(notFound.Result.Message.Message);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, SubmitUpdateUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            
            var validator = new SubmitUpdateUserCommandValidator();
            var result = await validator.ValidateAsync(command);
            if (!result.IsValid) return BadRequest(result.Errors);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:submit-update-user"));
            await endpoint.Send<SubmitUpdateUser>(new
            {
                command.Id,
                command.Username,
                command.Email,
                command.Password,
                command.FirstName,
                command.LastName ,
                command.Address,
                command.City,             
                command.Country,
                command.ZipCode,
            });

            return NoContent();
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
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