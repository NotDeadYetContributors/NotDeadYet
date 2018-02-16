using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NotDeadYet.Samples.AspNetCore
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
            services.AddMvc();

            AddHealthCheck(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseHealthCheck();
        }

        private IApplicationBuilder UseHealthCheck(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<HealthCheckMiddleware>();
        }


        //private IApplicationBuilder UseHealthCheck(IApplicationBuilder app, string path)
        //{
        //    if (app == null)
        //    {
        //        throw new ArgumentNullException(nameof(app));
        //    }

        //    return app.UseMiddleware<HealthCheckMiddleware>();
        //}

        public static IServiceCollection AddHealthCheck(IServiceCollection services)
        {
            Action<HealthCheckerBuilder> configureBuilder = builder => builder.WithHealthChecksFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

            return AddHealthCheck(services, configureBuilder);
        }

        public static IServiceCollection AddHealthCheck(IServiceCollection services, Action<HealthCheckerBuilder> configureHealthCheckerBuilder)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureHealthCheckerBuilder == null)
            {
                throw new ArgumentNullException(nameof(configureHealthCheckerBuilder));
            }


            var builder = new HealthCheckerBuilder();
            configureHealthCheckerBuilder(builder);

            // We add it as a singleton to that we are able to use it as part of the middleware pipeline
            return services.AddSingleton<IHealthChecker>(sp => builder.Build());
        }
    }
}
