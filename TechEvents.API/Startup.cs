using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TechEvents.API.Infrastructure.Repositories;

namespace TechEvents.API
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
            services.AddMvc(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<ApplicationDbContext>(context => { context.UseInMemoryDatabase("TechEventDB"); });

            services.AddTransient<ITechEventRepository, TechEventRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                                      "v1", new Swashbuckle.AspNetCore.Swagger.Info
                                      {
                                          Title = "TechEvent API",
                                          Version = "v1",
                                      });

                var xmlfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlfilefullpath = Path.Combine(AppContext.BaseDirectory, xmlfile);
                c.IncludeXmlComments(xmlfilefullpath);

                c.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = "Authorization",
                        Type = "apiKey",
                    });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });
            });

            services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://accounts.google.com";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://accounts.google.com",
                    ValidateAudience = true,
                    ValidAudience = "791542258089-grqp4q5bq075d6t4hsu6b1p28un8115i.apps.googleusercontent.com",
                    ValidateLifetime = true
                };
            });

            //services
            //  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //  .AddJwtBearer(options =>
            //  {
            //      options.Authority = "https://securetoken.google.com/my-techdemo-project";
            //      options.TokenValidationParameters = new TokenValidationParameters
            //      {
            //          ValidateIssuer = true,
            //          ValidIssuer = "https://securetoken.google.com/my-techdemo-project",
            //          ValidateAudience = true,
            //          ValidAudience = "my-techdemo-project",
            //          ValidateLifetime = true,                  
            //      };
            //  });

            services.AddMemoryCache();
            services.AddResponseCaching();

            services.AddProblemDetails(this.ConfigureProblemDetails);

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
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechEvent v1");
                c.RoutePrefix = string.Empty;
                c.OAuthClientId("791542258089-grqp4q5bq075d6t4hsu6b1p28un8115i.apps.googleusercontent.com");
            });
            app.UseProblemDetails();
            app.UseResponseCaching();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }

        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            options.Map<Exception>(ex =>
            {
                return new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError);
            });
        }
    }
}
