using GraphiQl;
using GraphQL.API.GraphqlCore;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechEvents.API.Infrastructure.Repositories;

namespace GraphQL.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<ApplicationDbContext>(context => { context.UseInMemoryDatabase("GraphQLDB"); });

            services.AddTransient<ITechEventRepository, TechEventRepository>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();

            services.AddSingleton<TechEventInfoType>();
            services.AddSingleton<ParticipantType>();
            services.AddSingleton<TechEventQuery>();
            services.AddSingleton<AddEventInputType>();
            services.AddSingleton<TechEventMutation>();

            var sp = services.BuildServiceProvider();
            services.AddSingleton<ISchema>(new TechEventSchema(new FuncDependencyResolver(type => sp.GetService(type))));
            //services.AddSingleton<IDependencyResolver>(c => new FuncDependencyResolver(type => c.GetRequiredService(type)));

            //services.AddGraphQL(o => { o.ExposeExceptions = false; })
            //    .AddGraphTypes(ServiceLifetime.Scoped);
        }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseGraphiQl("/graphql");
            //app.UseGraphQL<TechEventSchema>();
            //app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions());
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
