using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using DataMongoApi.Validation;
using DataMongoApi.Service.InterfaceService;

namespace DataMongoApi
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
            services.AddScoped<IClientConfiguration, ClientConfiguration>();

            services.Configure<SalonDatabaseSettings>(
        Configuration.GetSection(nameof(SalonDatabaseSettings)));

            services.AddSingleton<ISalonDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<SalonDatabaseSettings>>().Value);

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddTransient<ITreatmentService, TreatmentService>();
            services.AddTransient<IMerchantService, MerchantService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IOperatingHoursService, OperatingHoursService>();
            services.AddTransient<AppointmentService>();
            services.AddTransient<AvailableAppointmentService>();

            services.AddControllers()
                        .AddNewtonsoftJson(options => options.UseMemberCasing())
                        .AddFluentValidation(fv =>
                        fv.RegisterValidatorsFromAssemblyContaining<EmployeeDetailsValidator>()
                        .RegisterValidatorsFromAssemblyContaining<TreatmentDetailsValidator>()
                        .RegisterValidatorsFromAssemblyContaining<OperatingHoursDetailsValidatior>()
                        .RegisterValidatorsFromAssemblyContaining<ClientDetailsValidator>());

            ;


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.OperationFilter<AddRequiredHeaderParameter>();

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

         //   app.UseLogger();

            app.UseClientConfiguration();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
