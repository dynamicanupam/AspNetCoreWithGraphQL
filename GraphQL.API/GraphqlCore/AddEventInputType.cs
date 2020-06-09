using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.API.GraphqlCore
{
    public class AddEventInputType : InputObjectGraphType
    {
        public AddEventInputType()
        {
            Name = "AddEventInput";
            Field<NonNullGraphType<StringGraphType>>("eventName");
            Field<StringGraphType>("speaker");
            Field<NonNullGraphType<DateGraphType>>("eventDate");
        }
    }
}
