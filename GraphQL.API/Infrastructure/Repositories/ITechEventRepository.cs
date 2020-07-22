using GraphQL.API.Infrastructure.DBContext;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.API.Domain;

namespace GraphQL.API.Infrastructure.Repositories
{
    public interface ITechEventRepository
    {
        Task<TechEventInfo[]> GetTechEvents();
        Task<TechEventInfo> GetTechEventById(int id);
        Task<List<Participant>> GetParticipantInfoByEventId(int id);
        Task<TechEventInfo> AddTechEvent(NewTechEventRequest techEvent);
        Task<TechEventInfo> UpdateTechEvent(TechEventInfo techEvent);
        Task<bool> DeleteTechEventById(TechEventInfo techEvent);

    }
}
