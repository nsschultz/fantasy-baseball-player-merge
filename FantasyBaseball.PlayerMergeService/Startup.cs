using FantasyBaseball.PlayerMergeService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FantasyBaseball.PlayerMergeService
{
    /// <summary>The class that sets up all of the configuration for the service.</summary>
    public class Startup
    {
        /// <summary>Creates a new instance of the startup and sets up the configuration object.</summary>
        /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
        public Startup(IHostEnvironment env) =>
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

        /// <summary>Represents a set of key/value application configuration properties.</summary>
        public IConfiguration Configuration { get; }

        /// <summary>This method configures the HTTP request pipeline.</summary>
        /// <param name="app">The object to convert to a string.</param>
        /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
        public void Configure(IApplicationBuilder app, IHostEnvironment env) 
        {
            app.UseCors();
            app.UseHsts();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        /// <summary>This method adds the services to the container.</summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services) => 
            services
                .AddCors(options => options.AddDefaultPolicy(builder => 
                    builder.WithOrigins("http://*.schultz.local", "http://localhost").SetIsOriginAllowedToAllowWildcardSubdomains()))
                .AddSingleton(Configuration)
                .AddSingleton<IDataGetterService, DataGetterService>()
                .AddSingleton<IHealthCheckService, HealthCheckService>()
                .AddSingleton<IMergeService, MergeService>()
                .AddSingleton<IPlayerUpdaterService, PlayerUpdaterService>()
                .AddControllers();
    }
}