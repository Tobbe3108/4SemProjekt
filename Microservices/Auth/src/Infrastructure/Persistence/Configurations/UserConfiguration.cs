using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Domain.Entities.AuthUser>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.AuthUser> builder)
        {
            builder.Property(t => t.UserName)
                .IsUnicode()
                .IsRequired();
            
            builder.Property(t => t.Email)
                .IsUnicode()
                .IsRequired();
        }
    }
}