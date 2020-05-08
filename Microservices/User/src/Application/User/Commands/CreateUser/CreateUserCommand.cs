using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ToolBox.Bus.Interfaces;
using User.Application.Common.Interfaces;

namespace User.Application.User.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Guid>
    {
        [DefaultValue("Trut1936")] public string Username { get; set; }
        [DefaultValue("ThomasFBrandt@rhyta.com")] public string Email { get; set; }
        [DefaultValue("Thomas")] public string FirstName { get; set; }
        [DefaultValue("Brandt")] public string LastName { get; set; }
        [DefaultValue("Zxasqw12")] public string Password { get; set; }
    }

    class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateUserCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                NormalizedUserName = request.Username.ToUpperInvariant(),
                Password = request.Password
            };

            await _dbContext.Users.AddAsync(entity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return entity.Id;
            
        }
    }
}