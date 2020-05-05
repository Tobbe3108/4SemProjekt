using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resource.Application.Resource.Commands.CreateResource;
using Resource.Application.Resource.Queries.GetResources;

namespace WebApi.Controllers
{
    [Authorize]
    public class ResourceController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<ResourceVm>> Get()
        {
            return await Mediator.Send(new GetResourcesQuery());
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<ResourceVm>> Get(Guid id)
        // {
        //     return await Mediator.Send(new GetResourceQuery { Id = id });
        // }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateResourceCommand command)
        {
            return await Mediator.Send(command);
        }

        // [HttpPut("{id}")]
        // public async Task<ActionResult> Update(Guid id, UpdateResourceCommand command)
        // {
        //     if (id != command.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     await Mediator.Send(command);
        //
        //     return NoContent();
        // }
        //
        // [HttpDelete("{id}")]
        // public async Task<ActionResult> Delete(Guid id)
        // {
        //     await Mediator.Send(new DeleteResourceCommand {Id = id});
        //
        //     return NoContent();
        // }
    }
}