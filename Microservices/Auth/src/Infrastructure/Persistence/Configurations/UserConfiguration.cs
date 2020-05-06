using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Domain.Entities.User>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.User> builder)
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