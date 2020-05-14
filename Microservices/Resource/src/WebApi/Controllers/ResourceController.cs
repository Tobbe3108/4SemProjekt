using System;
using System.Threading.Tasks;
using Contracts.Resource;
using FluentValidation.Results;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Resource.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IRequestClient<GetResource> _getResourceRequestClient;
        private readonly IRequestClient<SubmitResource> _submitResourceRequestClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public ResourceController(IRequestClient<GetResource> getResourceRequestClient, IRequestClient<SubmitResource> submitResourceRequestClient, ISendEndpointProvider sendEndpointProvider)
        {
            _getResourceRequestClient = getResourceRequestClient;
            _submitResourceRequestClient = submitResourceRequestClient;
            _sendEndpointProvider = sendEndpointProvider;
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(SubmitResourceCommand command)
        {
            var validator = new SubmitResourceCommandValidator();
            var result = await validator.ValidateAsync(command);
            if (!result.IsValid) return BadRequest(result.Errors);
            
            var response = await _submitResourceRequestClient.GetResponse<SubmitResourceAccepted>(new
            {
                Id = Guid.NewGuid(),
                command.Name,
                command.Description,
                command.Available
            });
            
            return Ok(response.Message.Id);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceVm>> GetById(Guid id)
        {
            var (resourceVm, notFound) = await _getResourceRequestClient.GetResponse<ResourceVm, NotFound>(new
            {
                Id = id
            });
            return resourceVm.IsCompletedSuccessfully ? Ok(resourceVm.Result.Message.Resource) : Problem(notFound.Result.Message.Message);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, SubmitUpdateResourceCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            
            var validator = new SubmitUpdateResourceCommandValidator();
            var result = await validator.ValidateAsync(command);
            if (!result.IsValid) return BadRequest(result.Errors);
        
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:submit-update-resource"));
            await endpoint.Send<SubmitUpdateResource>(new
            {
                command.Id,
                command.Name,
                command.Description,
                command.Available
            });
        
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var request = new SubmitDeleteResourceCommand{ Id = id };
            var validator = new SubmitDeleteResourceCommandValidator();
            ValidationResult result = await validator.ValidateAsync(request);
            if (!result.IsValid) return BadRequest(result.Errors);
        
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:submit-delete-resource"));
            await endpoint.Send<SubmitDeleteResource>(new
            {
                Id = request.Id
            });
        
            return NoContent();
        }
    }
}