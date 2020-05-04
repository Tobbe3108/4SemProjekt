using Microsoft.EntityFrameworkCore;

namespace Resource.Data.Context
{
    public class ResourceDbContext : DbContext
    {
        public ResourceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Domain.Models.Resource> Resources { get; set; }
    }
}