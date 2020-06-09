using Newtonsoft.Json;
using System.Collections.Generic;
using TechEvents.API.Infrastructure.DBContext;

namespace TechEvents.API.Infrastructure.Repositories
{
    public class Participant
    {
        public Participant()
        {
            EventParticipants = new HashSet<EventParticipants>();
        }
        public int ParticipantId { get; set; }

        public string ParticipantName { get; set; }

        public string Email { get; set; }

        public int Phone { get; set; }

        [JsonIgnore]
        public virtual ICollection<EventParticipants> EventParticipants { get; set; }
    }
}
