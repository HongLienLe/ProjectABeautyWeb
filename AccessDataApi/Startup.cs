using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace AccessDataApi
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

            services.AddDbContext<ApplicationContext>(options =>

            options.UseSqlServer(
                         Configuration.GetConnectionString("ProjectABeautyDb"))
                    );

            services.AddScoped<IAvalibilityRepo, AvalibilityRepo>();
            services.AddScoped<IBookAppRepo, BookAppRepo>();
            services.AddScoped<IClientAccountRepo, ClientAccountRepo>();
            services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            services.AddScoped<IEmployeeTreatmentRepo, EmployeeTreatmentRepo>();
            services.AddScoped<IWorkScheduleRepo, WorkScheduleRepo>();

            services.AddControllers();

            services.AddSwaggerGen(c => c.SwaggerDoc(name: "v1", new OpenApiInfo {Title = "Api", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(
                c => { c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Api");
                    c.RoutePrefix = string.Empty;
                });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            context.CreateSeedData();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
