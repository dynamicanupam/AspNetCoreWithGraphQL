using System;
using System.Collections.Generic;

namespace GraphQL.API.Infrastructure.DBContext
{
    public partial class Participant
    {
        public Participant()
        {
            EventParticipants = new HashSet<EventParticipants>();
        }

        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<EventParticipants> EventParticipants { get; set; }
    }
}
