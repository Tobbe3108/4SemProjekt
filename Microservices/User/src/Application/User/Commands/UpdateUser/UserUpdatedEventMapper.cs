using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Application.Common.Interfaces;
using User.Domain.Entities;

namespace User.Application.User.Commands.UpdateUser
{
    public class UserUpdatedEventMapper : IEventMapper
    {
        public IEnumerable<OutboxMessage> Map(DbContext dbContext)
        {
            return dbContext
                    .ChangeTracker
                    .Entries<Domain.Entities.User>()
                    .Where(u => u.State == EntityState.Modified)
                    .Select(entry =>
                        new OutboxMessage(DateTime.Now,
                            new UserUpdatedEvent
                            {
                                Id = entry.Entity.Id,
                                Username = entry.Entity.Username,
                                Email = entry.Entity.Email,
                                Password = entry.Entity.Password
                            }));
            
        }
    }
}