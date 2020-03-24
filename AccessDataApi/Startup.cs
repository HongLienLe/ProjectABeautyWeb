using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AccessDataApi.Authentication;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using AutoMapper;


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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();


            ////// Use SQL Database if in Azure, otherwise, use SQLite
            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            //    services.AddDbContext<ApplicationContext>(options =>
            //            options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection")));
            //else
            services.AddDbContext<ApplicationContext>(options =>
                    options.UseSqlite("Data Source=localdatabase.db"));

            //services.AddEntityFrameworkSqlite()
            //  .AddDbContext<UserDbContext>(opt => opt.UseSqlite("Data Source =Userdatabase.db"));

            //services.AddIdentity<IdentityUser, IdentityRole>()
            //  .AddEntityFrameworkStores<UserDbContext>();

            //  Automatically perform database migration
            //services.BuildServiceProvider().GetService<ApplicationContext>().Database.Migrate();

            //services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            //var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            //var secret = Encoding.ASCII.GetBytes(token.Secret);

            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        // Validate the JWT Issuer (iss) claim
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(secret),

            //        // Validate the JWT Audience (aud) claim
            //        ValidateIssuer = false,
            //        ValidIssuer = token.Issuer,

            //        ValidateAudience = false,
            //        ValidAudience = token.Audience

            //    };
            //});

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            // services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddTransient<IAvailabilityRepo, AvailabilityRepo>();
            services.AddTransient<IBookAppointment, BookAppointment>();
            services.AddTransient<IClientAccountRepo, ClientAccountRepo>();
            services.AddTransient<IEmployeeRepo, EmployeeRepo>();
            services.AddTransient<IEmployeeTreatmentRepo, EmployeeTreatmentRepo>();
            services.AddTransient<IWorkScheduleRepo, WorkScheduleRepo>();
            services.AddTransient<ITreatmentRepo, TreatmentRepo>();
            services.AddScoped<IUserService, UserService>();

            //services.AddScoped<IUserManagementService, UserManagementService>();



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc
                  (name: "v1", new OpenApiInfo { Title = "Api", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

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

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

           // context.CreateSeedData();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
