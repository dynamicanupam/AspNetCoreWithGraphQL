
using System;
using System.ComponentModel.DataAnnotations;

namespace TechEvents.API.Domain
{
    public class NewTechEventRequest
    {
        [Required]
        public string EventName { get; set; }

        public string Speaker { get; set; }

        public DateTime EventDate { get; set; }
    }
}
