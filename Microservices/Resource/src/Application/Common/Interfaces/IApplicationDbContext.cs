using Resource.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Resource.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TodoList> TodoLists { get; set; }

        DbSet<TodoItem> TodoItems { get; set; }

        DbSet<Domain.Entities.Resource> Resources { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}