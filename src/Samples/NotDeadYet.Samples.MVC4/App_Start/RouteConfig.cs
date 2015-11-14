using System.Web.Mvc;
using System.Web.Routing;
using NotDeadYet.MVC4;

namespace NotDeadYet.Samples.MVC4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var healthChecker = new HealthCheckerBuilder()
                .WithHealthChecksFromAssemblies(typeof(MvcApplication).Assembly)
                .Build();

            routes.RegisterHealthCheck(healthChecker);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}