using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechEvents.API.Domain;
using TechEvents.API.Infrastructure.DBContext;

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

            LoadIntialData();
        }
        private void LoadIntialData()
        {
            if (!_context.TechEventInfo.Any())
            {
                var e1 = _context.TechEventInfo.Add(
                    new TechEventInfo
                    {
                        EventName = "ASP .NET Core Web API",
                        Speaker = "Anupam",
                        EventDate = DateTime.Today,
                    }
                    );

                var e2 = _context.TechEventInfo.Add(
                   new TechEventInfo
                   {
                       EventName = "Angular",
                       Speaker = "Pooja",
                       EventDate = DateTime.Today,
                   }
                   );
                var e3 = _context.TechEventInfo.Add(
                  new TechEventInfo
                  {
                      EventName = "GraphQL",
                      Speaker = "Anupam",
                      EventDate = DateTime.Now.AddDays(7),
                  }
                  );
                var p1 = _context.Participant.Add(
                    new Participant
                    {
                        ParticipantName = "Balaji G",
                        Email = "test@test.com",
                        Phone = 1234567890,
                    }
                    );

                var p2 = _context.Participant.Add(
                    new Participant
                    {
                        ParticipantName = "John G",
                        Email = "test@test.com",
                        Phone = 1234567890,
                    }
                    );

                _context.EventParticipants.AddRange(
                    new EventParticipants
                    {
                        EventId = e1.Entity.EventId,
                        ParticipantId = p1.Entity.ParticipantId,
                    },
                     new EventParticipants
                     {
                         EventId = e1.Entity.EventId,
                         ParticipantId = p2.Entity.ParticipantId,
                     },
                      new EventParticipants
                      {
                          EventId = e2.Entity.EventId,
                          ParticipantId = p2.Entity.ParticipantId,
                      }
                    );

                _context.SaveChanges();
            }
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

        //public async Task<TechEventResponse> GetTechEventInfoById(int id)
        //{
        //    List<Participant> participants = new List<Participant>();
        //    var tEvent = _context.TechEventInfo.FirstOrDefault(i => i.EventId == id);
        //    if (tEvent  != null)
        //    {
        //        participants = await (from ep in this._context.EventParticipants
        //                            join p in this._context.Participant on ep.ParticipantId equals p.ParticipantId
        //                            where ep.EventId == tEvent.EventId
        //                            select p).ToListAsync();
        //    }

        //    return new TechEventResponse { EventId = id, EventName = tEvent.EventName, Participants = participants };

        //}

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
