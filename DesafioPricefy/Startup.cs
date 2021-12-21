using DesafioPricefy.Dominio.Interfaces.CommandHandler;
using DesafioPricefy.Dominio.Interfaces.Infra;
using DesafioPricefy.Dominio.Interfaces.RepositorioEscrita;
using DesafioPricefy.Dominio.Interfaces.RepositorioLeitura;
using DesafioPricefy.Handler;
using DesafioPricefy.Repositorio.Escrita;
using DesafioPricefy.Repositorio.Leitura;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI;

namespace DesafioPricefy
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
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddScoped<IConnectionHandler, DatabaseConnection>();
            services.AddScoped<IUploadFileRepositorioEscrita, UploadFileRepositorioEscrita>();
            services.AddScoped<IUploadFileRepositorioLeitura, UploadFileRepositorioLeitura>();

            services.AddScoped<IUploadFileCommandHandler, UploadFileCommandHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
