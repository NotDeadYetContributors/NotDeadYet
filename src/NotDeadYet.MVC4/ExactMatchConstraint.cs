using System.Web;
using System.Web.Routing;

namespace NotDeadYet.MVC4
{
    internal class ExactMatchConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var isMatch = string.CompareOrdinal(route.Url.Trim('/'), httpContext.Request.RawUrl.Trim('/')) == 0;
            return isMatch;
        }
    }
}