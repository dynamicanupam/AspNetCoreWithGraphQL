using GraphQL.API.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.API.Models;


namespace GraphQL.API.Infrastructure.Repositories
{
    /// <summary>
    /// TechEventRepository.
    /// </summary>
    public class TechEventRepository : ITechEventRepository
    {

        /// <summary>
        /// The _context.
        /// </summary>
        private readonly TechEventDBContext _context;

        public TechEventRepository(TechEventDBContext context)
        {
            this._context = context;
        }

        public async Task<TechEventInfo> AddTechEventAsync(NewTechEventRequest techEvent)
        {
            var newEvent = new TechEventInfo { EventName = techEvent.EventName, Speaker = techEvent.Speaker, EventDate = techEvent.EventDate };
            var savedEvent = (await _context.TechEventInfo.AddAsync(newEvent)).Entity;
            await _context.SaveChangesAsync();
            return savedEvent;
        }

        public async Task<bool> DeleteTechEventAsync(TechEventInfo techEvent)
        {
            _context.TechEventInfo.Remove(techEvent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Participant>> GetParticipantInfoByEventIdAsync(int id)
        {
            return await(from ep in this._context.EventParticipants
                                 join p in this._context.Participant on ep.ParticipantId equals p.ParticipantId
                                 where ep.EventId == id
                                 select p).ToListAsync();
        }

        public async Task<TechEventInfo> GetTechEventByIdAsync(int id)
        {
            return await Task.FromResult( _context.TechEventInfo.FirstOrDefault(i => i.EventId == id));
        }

        public async Task<TechEventInfo[]> GetTechEventsAsync()
        {
            return await _context.TechEventInfo.ToArrayAsync();
        }

        public async Task<TechEventInfo> UpdateTechEventAsync(TechEventInfo techEvent)
        {
            var updatedEvent = (_context.TechEventInfo.Update(techEvent)).Entity;
            await _context.SaveChangesAsync();
            return updatedEvent;
        }
    }
}
