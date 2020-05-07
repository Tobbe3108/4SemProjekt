using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Application.Common.Interfaces;
using User.Domain.Entities;

namespace User.Application.User.Commands.CreateUser
{
    public class UserCreatedEventMapper : IEventMapper
    {
        public IEnumerable<OutboxMessage> Map(DbContext dbContext)
        {
            return dbContext
                .ChangeTracker
                .Entries<Domain.Entities.User>()
                .Where(u => u.State == EntityState.Added)
                .Select(entry =>
                    new OutboxMessage(DateTime.Now,
                        new UserCreatedEvent
                        {
                            Id = entry.Entity.Id,
                            Email = entry.Entity.Email,
                            Password = entry.Entity.Password,
                            Username = entry.Entity.Username
                        }));
        }
    }
}