using Microsoft.Owin;
using Owin;

namespace NotDeadYet.Owin
{
    public static class ExtensionMethods
    {
        public static void UseHealthCheck(this IAppBuilder app, IHealthChecker healthChecker,
            PathString? healthCheckPath = null)
        {
            app.Use<HealthCheckMiddleware>(healthChecker, healthCheckPath ?? new PathString("/healthcheck"));
        }
    }
}
