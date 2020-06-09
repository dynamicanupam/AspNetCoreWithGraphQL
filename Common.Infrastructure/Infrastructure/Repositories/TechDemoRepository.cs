using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechEvents.API.Domain;

namespace TechEvents.API.Infrastructure.Repositories
{
    /// <summary>
    /// TechEventRepository.
    /// </summary>
    public class TechEventRepository : ITechEventRepository
    {

        /// <summary>
        /// The _context.
        /// </summary>
        private readonly ApplicationDbContext _context;

        public TechEventRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<TechEventInfo> AddTechEvent(NewTechEventRequest techEvent)
        {
            var newEvent = new TechEventInfo { EventName = techEvent.EventName, Speaker = techEvent.Speaker };
            var savedEvent = (await _context.TechEventInfo.AddAsync(newEvent)).Entity;
            await _context.SaveChangesAsync();

            return savedEvent;
        }

        public async Task<bool> DeleteTechEventById(TechEventInfo techEvent)
        {
            _context.TechEventInfo.Remove(techEvent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TechEventInfo> GetTechEventById(int id)
        {
            return _context.TechEventInfo.FirstOrDefault(i => i.EventId == id);
        }

        public async Task<TechEventResponse> GetTechEventInfoById(int id)
        {
            List<Participant> participants = new List<Participant>();
            var tEvent = _context.TechEventInfo.FirstOrDefault(i => i.EventId == id);
            if (tEvent  != null)
            {
                participants = await (from ep in this._context.EventParticipants
                                    join p in this._context.Participant on ep.ParticipantId equals p.ParticipantId
                                    where ep.EventId == tEvent.EventId
                                    select p).ToListAsync();
            }

            return new TechEventResponse { EventId = id, EventName = tEvent.EventName, Participants = participants };

        }

        public async Task<TechEventInfo[]> GetTechEvents()
        {
            return _context.TechEventInfo.ToArray();
        }

        public async Task<TechEventInfo> UpdateTechEvent(TechEventInfo techEvent)
        {
            var updatedEvent = (_context.TechEventInfo.Update(techEvent)).Entity;
            await _context.SaveChangesAsync();

            return updatedEvent;
        }
    }
}
