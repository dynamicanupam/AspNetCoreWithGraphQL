using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechEvents.API.Infrastructure.Repositories;

namespace TechEvents.API.Infrastructure.EntityConfig
{
    public class ParticipantEntityConfig : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.ToTable("Participant");
            builder.HasKey(b => b.ParticipantId);
            builder.Property(b => b.ParticipantName);
            builder.Property(b => b.Email);
            builder.Property(b => b.Phone);
        }
    }
}
