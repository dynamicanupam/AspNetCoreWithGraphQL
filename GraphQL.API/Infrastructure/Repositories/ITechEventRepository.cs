using GraphQL.API.Infrastructure.DBContext;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.API.Models;

namespace GraphQL.API.Infrastructure.Repositories
{
    public interface ITechEventRepository
    {
        Task<TechEventInfo[]> GetTechEventsAsync();
        Task<TechEventInfo> GetTechEventByIdAsync(int id);
        Task<List<Participant>> GetParticipantInfoByEventIdAsync(int id);
        Task<TechEventInfo> AddTechEventAsync(NewTechEventRequest techEvent);
        Task<TechEventInfo> UpdateTechEventAsync(TechEventInfo techEvent);
        Task<bool> DeleteTechEventAsync(TechEventInfo techEvent);

    }
}
