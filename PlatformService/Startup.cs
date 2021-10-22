using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PlatformService.Data;

namespace PlatformService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            Console.WriteLine($"--> Using SqlServer DB");

            var connectionString = $"Server={Configuration["DBServerName"]}" +
                                    $";Initial Catalog={Configuration["DBName"]}" +
                                    $";User ID={Configuration["DBUserID"]}" +
                                    $";Password={Configuration["DBPassword"]};";

            Console.WriteLine($"--> Using Connection string {connectionString}");
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

            services.AddScoped<IPlatformRepo, PlatformRepo>();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
            });

            Console.WriteLine($"--> CommandService Endpoint {Configuration["CommandService"]}");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDb.PrePolulation(app);
        }
    }
}
