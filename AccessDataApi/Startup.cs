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
            services.AddCors();

            //// Use SQL Database if in Azure, otherwise, use SQLite
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                services.AddDbContext<ApplicationContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection")));
            else
                services.AddDbContext<ApplicationContext>(options =>
                        options.UseSqlite("Data Source=localdatabase.db"));


          //  Automatically perform database migration
            services.BuildServiceProvider().GetService<ApplicationContext>().Database.Migrate();

            services.AddTransient<IAvailabilityRepo, AvailabilityRepo>();
            services.AddTransient<IBookAppointment, BookAppointment>();
            services.AddTransient<IClientAccountRepo, ClientAccountRepo>();
            services.AddTransient<IEmployeeRepo, EmployeeRepo>();
            services.AddTransient<IEmployeeTreatmentRepo, EmployeeTreatmentRepo>();
            services.AddTransient<IWorkScheduleRepo, WorkScheduleRepo>();
            services.AddTransient<ITreatmentRepo, TreatmentRepo>();


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

            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.AllowAnyMethod();
            });

            app.UseSwagger();

            app.UseSwaggerUI(
                c => { c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Api");
                    c.RoutePrefix = string.Empty;
                });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //context.CreateSeedData();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
