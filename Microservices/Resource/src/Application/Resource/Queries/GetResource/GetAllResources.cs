using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Resource.Application.Common.Interfaces;

namespace Contracts.Resource
{
    public interface GetAllResources
    {
        
    }
    
    public class GetAllResourcesConsumer : IConsumer<GetAllResources>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetAllResourcesConsumer(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task Consume(ConsumeContext<GetAllResources> context)
        {
            try
            {
                var resources = await _dbContext.Resources.Include(r => r.Available).ToListAsync();
                await context.RespondAsync<ResourcesVm>(new
                {
                    Resources = resources
                });
            }
            catch (Exception e)
            {
                await context.RespondAsync<NotFound>(new
                {
                    Message = e.Message
                });
            }
        }
    }

    public interface ResourcesVm
    {
        public IList<global::Resource.Domain.Entities.Resource> Resources { get; set; }
    }
}