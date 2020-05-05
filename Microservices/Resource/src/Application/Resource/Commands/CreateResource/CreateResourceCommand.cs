using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Resource.Application.Common.Interfaces;

namespace Resource.Application.Resource.Commands.CreateResource
{
    public class CreateResourceCommand : IRequest<Guid>
    {
        public string Name { get; set; }
    }

    public class CreateResourceCommandHandler : IRequestHandler<CreateResourceCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateResourceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Resource
            {
                Name = request.Name
            };

            _context.Resources.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}