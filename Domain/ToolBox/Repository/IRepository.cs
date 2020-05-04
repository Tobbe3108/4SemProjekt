using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.Repository
{
    public interface IRepository<T> where T : IEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}