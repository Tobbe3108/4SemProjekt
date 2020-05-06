using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistence.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<Domain.Entities.UserRole>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.UserRole> builder)
        {
            builder.HasKey(ur => new {ur.UserId, ur.RoleId});
        }
    
    }
}