using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IRequestClient<GetAllResources> _getAllResourcesRequestClient;
        private readonly IRequestClient<GetResource> _getResourceRequestClient;
        private readonly IRequestClient<SubmitResource> _submitResourceRequestClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public ResourceController(IRequestClient<GetAllResources> getAllResourcesRequestClient, IRequestClient<GetResource> getResourceRequestClient, IRequestClient<SubmitResource> submitResourceRequestClient, ISendEndpointProvider sendEndpointProvider)
        {
            _getAllResourcesRequestClient = getAllResourcesRequestClient;
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

            command.Available.ForEach(dayAndTime => dayAndTime.Id = Guid.NewGuid());
            
            var response = await _submitResourceRequestClient.GetResponse<SubmitResourceAccepted>(new
            {
                Id = Guid.NewGuid(),
                command.Name,
                command.Description,
                command.Available
            });
            
            return Ok(response.Message.Id);
        }

        [HttpGet]
        public async Task<ActionResult<List<ResourceVm>>> Get()
        {
            var (resourceVmList, notFound) = await _getAllResourcesRequestClient.GetResponse<ResourcesVm, NotFound>(new {});
            return resourceVmList.IsCompletedSuccessfully ? Ok(resourceVmList.Result.Message.Resources) : Problem(notFound.Result.Message.Message);
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
        public async Task<IActionResult> Update(Guid id, SubmitUpdateResourceCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            
            var validator = new SubmitUpdateResourceCommandValidator();
            var result = await validator.ValidateAsync(command);
            if (!result.IsValid) return BadRequest(result.Errors);
            
            foreach (var dayAndTime in command.Available.Where(dayAndTime => dayAndTime.Id == Guid.Empty))
            {
                dayAndTime.Id = Guid.NewGuid();
            }
            
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
        public async Task<IActionResult> Delete(Guid id)
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