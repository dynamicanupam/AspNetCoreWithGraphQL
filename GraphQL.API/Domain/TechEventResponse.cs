using System;
using System.Collections.Generic;
using GraphQL.API.Infrastructure.DBContext;

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
