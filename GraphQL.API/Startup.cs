using GraphiQl;
using GraphQL.API.GraphqlCore;
using GraphQL.API.Infrastructure.DBContext;
using GraphQL.API.Infrastructure.Repositories;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddDbContext<TechEventDBContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("GraphQLDBConnection")));

            services.AddTransient<ITechEventRepository, TechEventRepository>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
                
            services.AddSingleton<TechEventInfoType>();
            services.AddSingleton<ParticipantType>();
            services.AddSingleton<TechEventQuery>();
            services.AddSingleton<TechEventInputType>();
            services.AddSingleton<TechEventMutation>();

            var sp = services.BuildServiceProvider();
            services.AddSingleton<ISchema>(new TechEventSchema(new FuncDependencyResolver(type => sp.GetService(type))));
   
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
