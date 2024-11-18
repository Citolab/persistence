using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Citolab.Persistence;
using Citolab.Persistence.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;


namespace MyApi
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
            // adding new List<Type> { typeof(SampleEntity) } to the option will make sure the collection is cached.
            // whats means that it keeps in cache as long as the API runs. It does get updates from 
            services.AddOpenApi();
            services.AddMongoDbPersistence("Example", Configuration.GetConnectionString("MongoDB"), new List<Type> { typeof(SampleEntity) });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI(options =>
              {
                  options.SwaggerEndpoint("/openapi/v1.json", "v1");
              });
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapOpenApi();
            });
            ;

        }
    }
}
