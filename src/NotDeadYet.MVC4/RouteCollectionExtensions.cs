using System.Web.Routing;

namespace NotDeadYet.MVC4
{
    public static class RouteCollectionExtensions
    {
        public static void RegisterHealthCheck(this RouteCollection routes, IHealthChecker healthChecker, string routeUrl = "healthcheck")
        {
            var route = new Route(routeUrl, new HealthCheckRouteHandler(healthChecker));
            routes.Add(route);
        }
    }
}