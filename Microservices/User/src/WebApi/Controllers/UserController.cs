using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Application.User.Commands.CreateUser;
using User.Application.User.Commands.UpdateUser;
using User.Application.User.Queries.GetUser;

namespace WebApi.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateUserCommand command)
        {
            return await Mediator.Send(command);
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
    }
}