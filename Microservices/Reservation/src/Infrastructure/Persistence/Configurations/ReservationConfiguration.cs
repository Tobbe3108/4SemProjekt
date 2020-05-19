﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservation.Infrastructure.Persistence.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Domain.Entities.Reservation>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Reservation> builder)
        {
            builder.Property(t => t.UserId)
                .IsRequired();
            builder.Property(t => t.ResourceId)
                .IsRequired();
            builder.Property(t => t.From)
                .IsRequired();
            builder.Property(t => t.To)
                .IsRequired();
        }
    }
}