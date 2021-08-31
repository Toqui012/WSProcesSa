using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa
{
    public class Startup
    {
        private readonly string AllowSpecificOrigins = "_allowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //DbContext Credentials
            services.AddDbContext<Models.ModelContext>(options =>
                 options.UseOracle(Configuration.GetConnectionString("Acceso")));

            //Cross-Origin(Cors)
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowSpecificOrigins, builder =>
                {
                    builder.WithOrigins(new string[]
                  {
                        "http://localhost:36493/api/region",
                        "http://localhost:36493/api/",
                        "http://localhost",
                        "file:///C:/Users/%C3%81lvaro/Desktop/Prueba/index.html",
                        "http://127.0.0.7:5500/index.html",
                        "http://127.0.0.7:5500"
                  }
                  )
                  .WithMethods("GET", "POST", "DELETE", "PUT")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(AllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
