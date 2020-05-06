using User.Application.TodoItems.Commands.CreateTodoItem;
using User.Application.TodoItems.Commands.DeleteTodoItem;
using User.Application.TodoItems.Commands.UpdateTodoItem;
using User.Application.TodoItems.Commands.UpdateTodoItemDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace User.WebUI.Controllers
{
    [Authorize]
    public class TodoItemsController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateTodoItemCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateTodoItemCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> UpdateItemDetails(int id, UpdateTodoItemDetailCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteTodoItemCommand {Id = id});

            return NoContent();
        }
    }
}