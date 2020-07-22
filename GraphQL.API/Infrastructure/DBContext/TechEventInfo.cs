using System;
using System.Collections.Generic;

namespace GraphQL.API.Infrastructure.DBContext
{
    public partial class TechEventInfo
    {
        public TechEventInfo()
        {
            EventParticipants = new HashSet<EventParticipants>();
        }

        public int EventId { get; set; }
        public string EventName { get; set; }
        public string Speaker { get; set; }
        public DateTime EventDate { get; set; }

        public virtual ICollection<EventParticipants> EventParticipants { get; set; }
    }
}
