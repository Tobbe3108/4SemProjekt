using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Resource.Application.Common.Interfaces;
using Resource.Application.TodoLists.Queries.GetTodos;
using Resource.Domain.Enums;

namespace Resource.Application.Resource.Queries.GetResources
{
    public class GetResourcesQuery :  IRequest<ResourceVm>
    {
        
    }

    public class GetResourcesQueryHandler : IRequestHandler<GetResourcesQuery, ResourceVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetResourcesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResourceVm> Handle(GetResourcesQuery request, CancellationToken cancellationToken)
        {
            return new ResourceVm
            {
                List = await _context.Resources
                    .ProjectTo<ResourceDto>(_mapper.ConfigurationProvider)
                    .OrderBy(t => t.Name)
                    .ToListAsync(cancellationToken)
            };
        }
    }
}