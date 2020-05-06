using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using User.Application.Common.Interfaces;

namespace User.Application.User.Queries.GetUser
{
    public class GetCurrentUserQuery : IRequest<UserVm>
    {
        
    }

    class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserVm>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _userService;

        public GetCurrentUserQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userService = userService;
        }
        public async Task<UserVm> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var normalizedUsername = _userService.Username.ToUpperInvariant();
            return new UserVm
            {
                User = await _dbContext.Users
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUsername, cancellationToken: cancellationToken)
            };
        }
    }
}