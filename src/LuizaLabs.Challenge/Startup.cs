using LuizaLabs.Challenge.Extensions;
using LuizaLabs.Challenge.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LuizaLabs.Challenge
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
            services.AddConfigurationOptions(Configuration);
            services.AddRepositories(Configuration);
            services.AddApiServices(Configuration);
            services.AddBusiness();
            services.AddCors(options =>
            {
                options.AddPolicy("All", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyOrigin();
                });
            });
            services.AddControllers();
            services.AddAppDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMigrations<AppDbContext>();
            app.UseDeveloperExceptionPage();
            app.UseAppDocumentation();

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors("All");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
