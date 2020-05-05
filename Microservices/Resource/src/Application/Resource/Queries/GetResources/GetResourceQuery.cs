using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Resource.Application.Common.Interfaces;

namespace Resource.Application.Resource.Queries.GetResources
{
    public class GetResourceQuery : IRequest<ResourceVm>
    {
        public Guid Id { get; set; }
    }

    public class GetResourceQueryHandler : IRequestHandler<GetResourceQuery, ResourceVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetResourceQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResourceVm> Handle(GetResourceQuery request, CancellationToken cancellationToken)
        {
            return new ResourceVm
            {
                Resource = await _context.Resources
                    .ProjectTo<ResourceDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken)
            };
        }
    }
}