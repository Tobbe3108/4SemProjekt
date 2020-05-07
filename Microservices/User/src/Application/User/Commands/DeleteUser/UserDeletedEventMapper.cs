using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Application.Common.Interfaces;
using User.Application.User.Commands.CreateUser;
using User.Application.User.Commands.UpdateUser;
using User.Domain.Entities;

namespace User.Application.User.Commands.DeleteUser
{
    public class UserDeletedEventMapper : IEventMapper
    {
        public IEnumerable<OutboxMessage> Map(DbContext dbContext)
        {
            return dbContext
                    .ChangeTracker
                    .Entries<Domain.Entities.User>()
                    .Where(u => u.State == EntityState.Deleted)
                    .Select(entry =>
                        new OutboxMessage(DateTime.Now,
                            new UserDeletedEvent()
                            {
                                Id = entry.Entity.Id
                            }));
        }
    }
}