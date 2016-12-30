using Nancy;
using NotDeadYet.Results;

namespace NotDeadYet.Nancy
{
    public class HealthCheckNancyModule : NancyModule
    {
        public static string EndpointName = "/healthcheck";

        private readonly IHealthChecker _healthChecker;

        public HealthCheckNancyModule(IHealthChecker healthChecker)
        {
            _healthChecker = healthChecker;

            Get[EndpointName] = _ => ExecuteHealthChecks();
        }

        private Response ExecuteHealthChecks()
        {
            var result = _healthChecker.Check();
            var statusCode = result.Status == HealthCheckStatus.NotOkay ? 503 : 200;

            var response = Response.AsJson(result)
                                   .WithStatusCode(statusCode)
                                   .WithContentType("text/plain")
                                   .WithHeader("Content-Disposition", "inline")
                                   .WithHeader("Cache-Control", "no-cache");
            return response;
        }
    }
}