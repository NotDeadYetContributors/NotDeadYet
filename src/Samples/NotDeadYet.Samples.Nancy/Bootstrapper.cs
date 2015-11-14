using Nancy.Bootstrapper;
using Nancy.Hosting.Aspnet;
using Nancy.TinyIoc;
using Newtonsoft.Json;

namespace NotDeadYet.Samples.Nancy
{
    public class Bootstrapper : DefaultNancyAspNetBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            container.Register<JsonSerializer, CustomJsonSerializer>();

            var notDeadYetAssembly = typeof (IHealthChecker).Assembly;
            var thisAssembly = typeof (Bootstrapper).Assembly;

            var healthChecker = new HealthCheckerBuilder()
                .WithHealthChecksFromAssemblies(thisAssembly, notDeadYetAssembly)
                .Build();

            container.Register(healthChecker);
        }
    }
}