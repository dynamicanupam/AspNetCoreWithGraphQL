using GraphQL.API.Infrastructure.DBContext;
using GraphQL.API.Infrastructure.Repositories;
using GraphQL.Types;

namespace GraphQL.API.GraphqlCore
{
    public class TechEventInfoType : ObjectGraphType<TechEventInfo>
    {
        public TechEventInfoType(ITechEventRepository repository)
        {
            Field(x => x.EventId).Description("Event id.");
            Field(x => x.EventName).Description("Event name.");
            Field(x => x.Speaker).Description("Speaker name.");
            Field(x => x.EventDate).Description("Event date.");

            Field<ListGraphType<ParticipantType>>(
              "participants",
              arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "eventId" }),
              resolve: context => repository.GetParticipantInfoByEventIdAsync(context.Source.EventId)
           );
        }
    }
}
