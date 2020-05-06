using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Domain.Entities.User>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.User> builder)
        {
            builder.Property(t => t.Username)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(t => t.Email)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(t => t.FirstName)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(t => t.LastName)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(t => t.Address)
                .HasMaxLength(200);
            builder.Property(t => t.City)
                .HasMaxLength(200);
            builder.Property(t => t.Country)
                .HasMaxLength(200);
            builder.Property(t => t.ZipCode)
                .HasMaxLength(200);
        }
    }
}