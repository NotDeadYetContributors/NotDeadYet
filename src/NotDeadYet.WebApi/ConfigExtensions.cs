using System.Web.Http;
using System.Web.Http.Routing;

namespace NotDeadYet.WebApi
{
    public static class ConfigExtensions
    {
        public static void RegisterHealthCheck(this HttpConfiguration config, IHealthChecker healthChecker, string routeUrl = "healthcheck")
        {
            var defaults = new HttpRouteValueDictionary();
            var constraint = new ExactMatchConstraint();
            var constraints = new HttpRouteValueDictionary {{routeUrl, constraint}};
            config.Routes.MapHttpRoute("healthcheck", routeUrl, defaults, constraints, new HealthCheckMessageHandler(healthChecker));
        }
    }
}