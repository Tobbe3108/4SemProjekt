using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using ToolBox.Events;

namespace User.Infrastructure.Persistence.Configurations
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<Domain.Entities.OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.OutboxMessage> builder)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };

            builder
                .HasKey(e => e.Id);
            builder
                .Property(e => e.Event)
                .HasColumnType("json")
                .HasConversion(
                    e => JsonConvert.SerializeObject(e, settings),
                    e => JsonConvert.DeserializeObject<BaseEvent>(e, settings));
        }
    }
}