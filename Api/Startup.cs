using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Services;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using Api.Models.Identity;
using Api.Contexts;
using Api.Security;
using InfrastructureDapper.Repositories;
using InfrastructureEF.Repositories;
using InfrastructureEF.Contexts;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //IdentityModelEventSource.ShowPII = true;

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
                });
            });

            var connectionString = Configuration.GetSection("ConnectionStrings:ConsultorioDB").Get<string>();

            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);            

            services.AddDbContext<IdentityContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("ConsultorioDB")));

            services.AddDefaultIdentity<User>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddDefaultTokenProviders();

            //JWT
            var jwtSettingsSection = Configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.ValidoEm,
                    ValidIssuer = jwtSettings.Emissor
                };
            });

            services.AddScoped<PacienteService>();
            services.AddScoped<ConsultaService>();
            // DAPPER
            //services.AddScoped<IRepositoryPaciente>(x => new InfrastructureDapper.Repositories.PacienteRepository(connectionString));
            //services.AddScoped<IRepositoryConsulta>(x => new InfrastructureDapper.Repositories.ConsultaRepository(connectionString));
            // EF       
            services.AddDbContext<DataContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("ConsultorioDB")));
            services.AddScoped<IRepositoryPaciente, InfrastructureEF.Repositories.PacienteRepository>();
            services.AddScoped<IRepositoryConsulta, InfrastructureEF.Repositories.ConsultaRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
