using System.Web.Http;

namespace NotDeadYet.WebApi
{
    public static class ConfigExtensions
    {
        public static void RegisterHealthCheck(this HttpConfiguration config, IHealthChecker healthChecker, string routeUrl = "healthcheck")
        {
            config.Routes.MapHttpRoute("healthcheck", routeUrl, null, null, new HealthCheckMessageHandler(healthChecker));
        }
    }
}