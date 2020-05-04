using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToolBox.Models;

namespace ToolBox.Repository
{
    public abstract class AbstractRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly DbContext _context;
        protected readonly DbSet<T> Entities;

        protected AbstractRepository(DbContext context)
        {
            _context = context;
            Entities = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await Entities.SingleOrDefaultAsync(s => s.Id == id);
        }

        public virtual async Task AddAsync(T entity)
        {
            await Entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await Entities.SingleOrDefaultAsync(s => s.Id == id);
            if (entity != null) Entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}