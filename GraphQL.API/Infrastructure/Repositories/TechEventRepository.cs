using GraphQL.API.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.API.Domain;


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

        public async Task<TechEventInfo> AddTechEvent(NewTechEventRequest techEvent)
        {
            var newEvent = new TechEventInfo { EventName = techEvent.EventName, Speaker = techEvent.Speaker, EventDate = techEvent.EventDate };
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

        public async Task<List<Participant>> GetParticipantInfoByEventId(int id)
        {
            return await(from ep in this._context.EventParticipants
                                 join p in this._context.Participant on ep.ParticipantId equals p.ParticipantId
                                 where ep.EventId == id
                                 select p).ToListAsync();
        }

        public async Task<TechEventInfo> GetTechEventById(int id)
        {
            return await Task.FromResult( _context.TechEventInfo.FirstOrDefault(i => i.EventId == id));
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
