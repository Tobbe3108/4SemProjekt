using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using User.Application.Common.Interfaces;

namespace User.Application.User.Queries.GetUser
{
    public class GetUserQuery : IRequest<UserVm>
    {
        public Guid Id { get; set; }
    }

    class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserVm>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<UserVm> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return new UserVm
            {
                User = await _dbContext.Users
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken: cancellationToken)
            };
        }
    }
}