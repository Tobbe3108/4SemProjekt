using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Resource.Application.Common.Exceptions;
using Resource.Application.Common.Interfaces;

namespace Resource.Application.Resource.Commands.DeleteResource
{
    public class DeleteResourceCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteResourceCommandHandler : IRequestHandler<DeleteResourceCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteResourceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteResourceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Resources.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Resource), request.Id);
            }

            _context.Resources.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}