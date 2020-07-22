using GraphQL.API.Infrastructure.Repositories;
using GraphQL.Types;
using GraphQL.API.Domain;

namespace GraphQL.API.GraphqlCore
{
    public class TechEventMutation : ObjectGraphType<object>
    {
        public TechEventMutation(ITechEventRepository repository)
        {
            Name = "TechEventMutation";

            Field<TechEventInfoType>(
                "addTechEvent",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AddEventInputType>> { Name = "techEventInput" }
                ),
                resolve: context =>
                {
                    var techEventInput = context.GetArgument<NewTechEventRequest>("techEventInput");
                    return repository.AddTechEvent(techEventInput);
                });
        }
    }
}
