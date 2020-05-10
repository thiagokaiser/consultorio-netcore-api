using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Core.Models.Identity;
using Core.Security;
using Core.Services;
using Core.Interfaces;
using InfrastructureEF.Contexts;
using InfrastructureEF.Repositories;

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
            services.AddDbContext<DataContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("ConsultorioDB")));

            services.AddScoped<AuthService>();
            services.AddScoped<PacienteService>();
            services.AddScoped<ConsultaService>();
            services.AddScoped<IRepositoryAuth, AuthRepository>();

            // DAPPER
            //services.AddScoped<IRepositoryPaciente>(x => new InfrastructureDapper.Repositories.PacienteRepository(connectionString));
            //services.AddScoped<IRepositoryConsulta>(x => new InfrastructureDapper.Repositories.ConsultaRepository(connectionString));
            // EF            
            services.AddScoped<IRepositoryPaciente, InfrastructureEF.Repositories.PacienteRepository>();
            services.AddScoped<IRepositoryConsulta, InfrastructureEF.Repositories.ConsultaRepository>();

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
