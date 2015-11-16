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

            var thisAssembly = typeof (MvcApplication).Assembly;

            var healthChecker = new HealthCheckerBuilder()
                .WithHealthChecksFromAssemblies(thisAssembly)
                .Build();

            routes.RegisterHealthCheck(healthChecker);

            routes.MapRoute("Default", "{controller}/{action}/{id}", new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}