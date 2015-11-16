using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NotDeadYet.Results;

namespace NotDeadYet.WebApi
{
    public class HealthCheckMessageHandler : HttpMessageHandler
    {
        private readonly IHealthChecker _healthChecker;

        public HealthCheckMessageHandler(IHealthChecker healthChecker)
        {
            _healthChecker = healthChecker;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             var result = _healthChecker.Check();

                                             var json = JsonConvert.SerializeObject(result, Formatting.Indented, new StringEnumConverter());
                                             var statusCode1 = result.Status == HealthCheckStatus.Okay ? HttpStatusCode.OK : HttpStatusCode.ServiceUnavailable;

                                             var response = request.CreateResponse(statusCode1);
                                             response.Content = new StringContent(json);
                                             response.Headers.CacheControl = new CacheControlHeaderValue
                                                                             {
                                                                                 NoCache = true
                                                                             };

                                             return response;
                                         },
                                         cancellationToken);
        }
    }
}