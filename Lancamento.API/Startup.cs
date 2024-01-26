using Lancamento.API.Application.Implementations;
using Lancamento.API.Application.Interfaces;
using Lancamento.API.Domain.Interfaces;
using Lancamento.API.Infra.Data.AutoMapper;
using Lancamento.API.Infra.Data.Context;
using Lancamento.API.Infra.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Lancamento.API
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lancamento.API", Version = "v1" });
            });

            services.AddDbContext<LancamentoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ILactoRepository, LactoRepository>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ILactoService, LactoService>();
            services.AddSingleton<IQueueService, QueueService>();
            services.AddAutoMapper(typeof(AutoMappings));

            services.Configure<QueueConfig>(Configuration.GetSection("RabbitMQ"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lancamento.API v1"));
            }

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
