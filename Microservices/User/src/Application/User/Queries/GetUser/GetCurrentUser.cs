﻿using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using User.Application.Common.Interfaces;

namespace Contracts.User
{
    public interface GetCurrentUser
    {
        public string Username { get; set; }
    }
    
    public class GetCurrentUserConsumer : IConsumer<GetCurrentUser>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetCurrentUserConsumer(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<GetCurrentUser> context)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(context.Message.Username))
                {
                    throw new ArgumentNullException();
                }

                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == context.Message.Username);
   
                await context.RespondAsync<UserVm>(new
                {
                    User = user
                });
            }
            catch (Exception e)
            {
                await context.RespondAsync<NotFound>(new
                {
                    Message = e.Message
                });
            }
            
        }
    }
}