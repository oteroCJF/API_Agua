using Agua.Persistence.Database;
using Agua.Service.Queries.Queries.CedulasEvaluacion;
using Agua.Service.Queries.Queries.Contratos;
using Agua.Service.Queries.Queries.Convenios;
using Agua.Service.Queries.Queries.Cuestionarios;
using Agua.Service.Queries.Queries.Entregables;
using Agua.Service.Queries.Queries.EntregablesContratacion;
using Agua.Service.Queries.Queries.Facturas;
using Agua.Service.Queries.Queries.Firmantes;
using Agua.Service.Queries.Queries.Incidencias;
using Agua.Service.Queries.Queries.LogCedulas;
using Agua.Service.Queries.Queries.LogEntregables;
using Agua.Service.Queries.Queries.LogOficios;
using Agua.Service.Queries.Queries.Oficios;
using Agua.Service.Queries.Queries.Repositorios;
using Agua.Service.Queries.Queries.Respuestas;
using Agua.Service.Queries.Queries.ServiciosContrato;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Agua.Api
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
            services.AddDbContext<ApplicationDbContext>(opts => {
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
               x => x.MigrationsHistoryTable("__EFMigrationHistory", "Agua")
               );
            });

            services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; options.JsonSerializerOptions.PropertyNameCaseInsensitive = false; });

            services.AddMediatR(Assembly.Load("Agua.Service.EventHandler"));

            services.AddTransient<ICuestionariosQueryService, CuestionarioQueryService>();
            services.AddTransient<IFirmantesQueryService, FirmanteQueryService>();
            services.AddTransient<IRepositorioQueryService, RepositorioQueryService>();
            services.AddTransient<IFacturasQueryService, FacturaQueryService>();
            services.AddTransient<ICedulasQueryService, CedulasQueryService>();
            services.AddTransient<IRespuestasQueryService, RespuestaQueryService>();
            services.AddTransient<IContratosQueryService, ContratoQueryService>();
            services.AddTransient<IConvenioQueryService, ConvenioQueryService>();
            services.AddTransient<IServicioContratoQueryService, ServicioContratoQueryService>();
            services.AddTransient<IIncidenciasQueryService, IncidenciaQueryService>();
            services.AddTransient<IEntregableQueryService, EntregableQueryService>();
            services.AddTransient<IEntregableContratacionQueryService, EntregableContratacionQueryService>();
            services.AddTransient<ILogCedulasQueryService, LogCedulaQueryService>();
            services.AddTransient<ILogEntregablesQueryService, LogEntregableQueryService>();
            services.AddTransient<ILogOficioQueryService, LogOficioQueryService>();
            services.AddTransient<IOficioQueryService, OficioQueryService>();

            services.AddControllers();

            var secretKey = Encoding.ASCII.GetBytes(
               Configuration.GetValue<string>("SecretKey")
           );

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
