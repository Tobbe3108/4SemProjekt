using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Resource.Application.Common.Exceptions;
using Resource.Application.Common.Interfaces;

namespace Resource.Application.Resource.Commands.UpdateResource
{
    public class UpdateResourceCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    class UpdateResourceCommandHandler : IRequestHandler<UpdateResourceCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateResourceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateResourceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Resources.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Resource), request.Id);
            }

            entity.Name = request.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}