using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechEvents.API.Domain;
using TechEvents.API.Infrastructure.Repositories;

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
