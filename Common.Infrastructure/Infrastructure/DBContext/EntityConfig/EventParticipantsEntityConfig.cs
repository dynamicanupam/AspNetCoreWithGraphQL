using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechEvents.API.Infrastructure.DBContext;
using TechEvents.API.Infrastructure.Repositories;

namespace Common.Infrastructure.EntityConfig
{
    public class EventParticipantsEntityConfig : IEntityTypeConfiguration<EventParticipants>
    {
        public void Configure(EntityTypeBuilder<EventParticipants> builder)
        {
            builder.ToTable("EventParticipants");
            builder.HasKey(b => b.EventParticipantId);

            builder.HasOne(p => p.Participant)
                   .WithMany(e => e.EventParticipants)
                   .HasForeignKey(p => p.ParticipantId);

            builder.HasOne(e => e.TechEventInfo)
                   .WithMany(b => b.EventParticipants)
                   .HasForeignKey(e => e.EventId);
        }
    }
}
