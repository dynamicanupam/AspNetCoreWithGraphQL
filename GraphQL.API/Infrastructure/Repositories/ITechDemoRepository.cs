using System.Collections.Generic;
using System.Threading.Tasks;
using TechEvents.API.Domain;

namespace TechEvents.API.Infrastructure.Repositories
{
    public interface ITechEventRepository
    {

        Task<TechEventInfo[]> GetTechEvents();

       Task<TechEventInfo> AddTechEvent(NewTechEventRequest techEvent);

        Task<TechEventInfo> GetTechEventById(int id);

        //Task<TechEventResponse> GetTechEventInfoById(int id);

        Task<List<Participant>> GetParticipantInfoByEventId(int id);

        Task<TechEventInfo> UpdateTechEvent(TechEventInfo techEvent);

        Task<bool> DeleteTechEventById(TechEventInfo techEvent);

    }
}
