using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToolBox.Models;
using ToolBox.Repository;

namespace ToolBox.Service
{
    public abstract class AbstractService<T> : IService<T> where T : IEntity
    {
        private readonly IRepository<T> _repository;

        protected AbstractService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}