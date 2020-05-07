using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ToolBox.Bus.Interfaces;
using User.Application.Common.Exceptions;
using User.Application.Common.Interfaces;

namespace User.Application.User.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEventBus _eventBus;

        public DeleteUserCommandHandler(IApplicationDbContext context, IEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }
        
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.User), request.Id);
            }

            _context.Users.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            //TODO Transactional Outbox
            //TODO Send password with event
            _eventBus.PublishEvent(new UserDeletedEvent
            {
                Id = request.Id
            });
            
            return Unit.Value;
            
        }
    }
}