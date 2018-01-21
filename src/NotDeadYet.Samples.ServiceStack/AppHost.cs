using Funq;
using ServiceStack;

namespace NotDeadYet.Samples.ServiceStack
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("NotDeadYet.Sample.ServiceStack", typeof(AppHost).Assembly, typeof(NotDeadYet.ServiceStack.HealthCheck).Assembly)
        {
        }

        public override void Configure(Container container)
        {
            //Not Dead Yet Assembly Registration
            var thisAssembly = typeof(AppHost).Assembly;
            var notDeadYetAssembly = typeof(IHealthChecker).Assembly;

            var healthChecker = new HealthCheckerBuilder()
                .WithHealthChecksFromAssemblies(thisAssembly, notDeadYetAssembly)
                .Build();

            container.Register(healthChecker);
        }
    }
}