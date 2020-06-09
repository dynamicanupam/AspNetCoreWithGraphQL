using TechEvents.API.Infrastructure.Repositories;

namespace TechEvents.API.Infrastructure.DBContext
{
    public class EventParticipants
    {
        public int EventParticipantId { get; set; }
        public int EventId { get; set; }

        public int ParticipantId { get; set; }

        public virtual TechEventInfo TechEventInfo { get; set; }
        public virtual Participant Participant { get; set; }
    }
}
