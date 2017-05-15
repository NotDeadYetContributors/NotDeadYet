using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace NotDeadYet.WebApi
{
    internal class ExactMatchConstraint : IHttpRouteConstraint
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            var isMatch = request.RequestUri.AbsolutePath.EndsWith(route.RouteTemplate);
            return isMatch;
        }
    }
}