using Microsoft.Owin;
using Owin;
using NotDeadYet.Owin;

[assembly: OwinStartup(typeof(NotDeadYet.Samples.Owin.Startup))]

namespace NotDeadYet.Samples.Owin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureHealthCheck(app);
        }

        private void ConfigureHealthCheck(IAppBuilder app)
        {
            var healthChecker = new HealthCheckerBuilder()
                .WithHealthChecksFromAssemblies(typeof(Startup).Assembly)
                .Build();

            app.UseHealthCheck(healthChecker);
        }
    }
}
