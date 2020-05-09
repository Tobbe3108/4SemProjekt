﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ToolBox.Bus.Interfaces;
using User.Application.Common.Exceptions;
using User.Application.Common.Interfaces;
using User.Application.Common.Mappings;

namespace User.Application.User.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest, IMapFrom<Domain.Entities.User>
    {
        [DefaultValue("")] public Guid Id { get; set; }
        [DefaultValue("Trut1936")] public string Username { get; set; }
        [DefaultValue("ThomasFBrandt@rhyta.com")] public string Email { get; set; }
        [DefaultValue("Zxasqw12")] public string Password { get; set; }
        [DefaultValue("Thomas")] public string FirstName { get; set; }
        [DefaultValue("Brandt")] public string LastName { get; set; }
        [DefaultValue("Mølleløkken 47")] public string Address { get; set; }
        [DefaultValue("Odder")] public string City { get; set; }
        [DefaultValue("Denmark")] public string Country { get; set; }
        [DefaultValue("9300")] public int? ZipCode { get; set; }
    }

    class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEventBus _eventBus;

        public UpdateUserCommandHandler(IApplicationDbContext dbContext, IEventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }
        
        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.User), request.Id);
            }

            entity.Address = request.Address;
            entity.City = request.City;
            entity.Country = request.Country;
            entity.Email = request.Email;
            entity.Username = request.Email;
            entity.NormalizedUserName = request.Username.ToUpperInvariant();
            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.ZipCode = request.ZipCode;
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
            
        }
    }
}