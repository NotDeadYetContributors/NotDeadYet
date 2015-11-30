using System.Web.Routing;

namespace NotDeadYet.MVC4
{
    public static class RouteCollectionExtensions
    {
        public static void RegisterHealthCheck(this RouteCollection routes, IHealthChecker healthChecker, string routeUrl = "healthcheck")
        {
            var constraint = new ExactMatchConstraint();
            var defaults = new RouteValueDictionary();
            var constraints = new RouteValueDictionary {{routeUrl, constraint}};

            var route = new Route("healthcheck", defaults, constraints, new HealthCheckRouteHandler(healthChecker));
            routes.Add(route);
        }
    }
}