using System;
using System.Collections.Generic;
using TechEvents.API.Infrastructure.Repositories;

namespace TechEvents.API.Domain
{
    public class TechEventResponse
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }

        public List<Participant> Participants { get; set; }
    }
}
