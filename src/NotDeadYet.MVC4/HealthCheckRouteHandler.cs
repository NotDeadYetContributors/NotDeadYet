using System.Web;
using System.Web.Routing;

namespace NotDeadYet.MVC4
{
    public class HealthCheckRouteHandler : IRouteHandler
    {
        private readonly IHealthChecker _healthChecker;

        public HealthCheckRouteHandler(IHealthChecker healthChecker)
        {
            _healthChecker = healthChecker;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new HealthCheckHttpHandler(_healthChecker);
        }
    }
}