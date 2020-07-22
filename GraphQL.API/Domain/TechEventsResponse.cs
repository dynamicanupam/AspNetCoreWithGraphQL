
using GraphQL.API.Infrastructure.DBContext;


namespace TechEvents.API.Domain
{
    public class TechEventsResponse
    {
        public int TotalNoOfEvents { get; set; }
        public TechEventInfo[] EventInfo { get; set; }
    }
}
