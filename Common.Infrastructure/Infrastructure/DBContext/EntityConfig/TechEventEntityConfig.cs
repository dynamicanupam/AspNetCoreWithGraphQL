using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechEvents.API.Infrastructure.Repositories;

namespace TechEvents.API.Infrastructure.EntityConfig
{
    public class TechEventEntityConfig : IEntityTypeConfiguration<TechEventInfo>
    {
        public void Configure(EntityTypeBuilder<TechEventInfo> builder)
        {
            builder.ToTable("TechEventInfo");
            builder.HasKey(b => b.EventId);
            builder.Property(b => b.EventName);
            builder.Property(b => b.Speaker);
           
        }
    }
}
