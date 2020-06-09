
using TechEvents.API.Infrastructure.Repositories;

namespace TechEvents.API.Domain
{
    public class TechEventsResponse
    {
        public int TotalNoOfEvents { get; set; }
        public TechEventInfo[] EventInfo { get; set; }
    }
}
