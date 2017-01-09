using NotDeadYet.Results;
using ServiceStack;
using System.Net;

namespace NotDeadYet.ServiceStack
{
    public class HealthCheckService : Service
    {
        private readonly IHealthChecker _healthChecker;

        public HealthCheckService(IHealthChecker healthChecker)
        {
            _healthChecker = healthChecker;
        }

        public object Get(HealthCheck request)
        {
            var result = _healthChecker.Check();
            var statusCode = (result.Status == HealthCheckStatus.NotOkay)
                ? HttpStatusCode.ServiceUnavailable
                : HttpStatusCode.OK;

            return new HttpResult(result, statusCode);
        }
    }

    [Api("Health Check")]
    [Route("/healthcheck", Verbs = "GET", Summary = "Checks if the website is online", Notes = "Health Check")]
    public class HealthCheck
    {
    }
}