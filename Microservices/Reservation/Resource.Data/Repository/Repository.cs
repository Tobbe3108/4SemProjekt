using Resource.Data.Context;
using Resource.Domain.Models;
using ToolBox.Repository;

namespace Resource.Data.Repository
{
    public class Repository<T> : AbstractRepository<T> where T : BaseEntity
    {
        public Repository(ResourceDbContext context) : base(context)
        {
        }
    }
}