using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TechEvents.API.Infrastructure.DBContext;

namespace TechEvents.API.Infrastructure.Repositories
{
    public class TechEventInfo
    {
        public TechEventInfo()
        {
            EventParticipants = new HashSet<EventParticipants>();
        }
        public int EventId { get; set; }

        [Required]
        public string EventName { get; set; }

        public string Speaker { get; set; }

        [JsonIgnore]
        public virtual ICollection<EventParticipants> EventParticipants { get; set; }
    }
}
