using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToolBox.Service;

namespace Resource.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ReservationController : Controller
    {
        private readonly IService<Domain.Models.Resource> _resourceService;

        public ReservationController(IService<Domain.Models.Resource> resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _resourceService.GetAllAsync();
                if (result == null) return new BadRequestResult();
                if (!result.Any()) return new NotFoundResult();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(503, e.Message); // Returns Service Unavailable
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            try
            {
                var result = await _resourceService.GetByIdAsync(id);
                if (result == null) return new NotFoundResult();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(503, e.Message); // Returns Service Unavailable
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Domain.Models.Resource resource)
        {
            if (resource == null) return BadRequest();
            try
            {
                await _resourceService.UpdateAsync(resource);
                return Ok(resource);
            }
            catch (Exception e)
            {
                return StatusCode(503, e.Message); // Returns Service Unavailable
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Domain.Models.Resource resource)
        {
            if (resource == null) return BadRequest();
            try
            {
                await _resourceService.AddAsync(resource);
                return Ok(resource);
            }
            catch (Exception e)
            {
                return StatusCode(503, e.Message); // Returns Service Unavailable
            }
        }
    }
}