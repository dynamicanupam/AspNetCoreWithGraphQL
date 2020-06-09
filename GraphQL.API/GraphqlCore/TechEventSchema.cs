using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.API.GraphqlCore
{
    public class TechEventSchema : Schema
    {
        public TechEventSchema(IDependencyResolver resolver)
        {
            Query = resolver.Resolve<TechEventQuery>();
            Mutation = resolver.Resolve<TechEventMutation>();
            DependencyResolver = resolver;
        }

    }
}
