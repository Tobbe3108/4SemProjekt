using Resource.Domain.Models;
using ToolBox.Repository;
using ToolBox.Service;

namespace Resource.Application.Services
{
    public class Service<T> : AbstractService<T> where T : BaseEntity
    {
        public Service(IRepository<T> repository) : base(repository)
        {
        }
    }
}