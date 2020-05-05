using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Infrastructure
{
    public class AuthRepository
    {
        private readonly AuthDbContext _authDbContext;

        public AuthRepository(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task Create(AuthUser user)
        {
            _authDbContext.Add(user);
            await _authDbContext.SaveChangesAsync();
        }

        public async Task<AuthUser> GetUserFromUserNameOrEmailAsync(string userNameOrEmail)
        {
            AuthUser authUser = await _authDbContext.AuthUsers.Include(u => u.UserRoles).ThenInclude(u => u.Role).SingleOrDefaultAsync(u => u.UserName == userNameOrEmail || u.Email == userNameOrEmail);
            return authUser;
        }

        public async Task UpdateUser(AuthUser user)
        {
            _authDbContext.AuthUsers.Update(user);
            await _authDbContext.SaveChangesAsync();
        }

        public async Task<AuthUser> GetUserFromId(Guid id)
        {
            return await _authDbContext.AuthUsers.SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task Delete(AuthUser user)
        {
            _authDbContext.Remove(user);
            await _authDbContext.SaveChangesAsync();
        }
    }
}
