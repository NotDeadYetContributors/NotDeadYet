using System;
using System.Diagnostics;
using System.Linq;
using NotDeadYet.Results;

namespace NotDeadYet
{
    internal class HealthChecker : IHealthChecker
    {
        private static class Messages
        {
            public const string AllOkayMessage = "All okay";
            public const string OneOrMoreHealthChecksFailedMessage = "One or more health checks failed";
            public const string NoHealthCheckersProvidedMessage = "No health checks were provided to the health checker.";
            public const string CannotCreateHealthCheckersMessage = "A catastrophic failure occurred. We can't even load our health checks!";
        }

        private readonly HealthCheckerBuilder.GetHealthChecks _healthChecksFunc;
        private readonly HealthCheckerBuilder.LogError _logException;

        public HealthChecker(HealthCheckerBuilder.GetHealthChecks healthChecksFunc, HealthCheckerBuilder.LogError logException)
        {
            _healthChecksFunc = healthChecksFunc;
            _logException = logException;
        }

        public HealthCheckOutcome Check()
        {
            IHealthCheck[] healthChecks;
            try
            {
                healthChecks = _healthChecksFunc();
            }
            catch (Exception ex)
            {
                _logException(ex, Messages.CannotCreateHealthCheckersMessage);
                return new HealthCheckOutcome(HealthCheckStatus.NotOkay, Messages.CannotCreateHealthCheckersMessage, new IndividualHealthCheckResult[0]);
            }

            if (healthChecks.Length == 0)
            {
                return new HealthCheckOutcome(HealthCheckStatus.NotOkay, Messages.NoHealthCheckersProvidedMessage, new IndividualHealthCheckResult[0]);
            }

            var individualResults = healthChecks
                .AsParallel()
                .AsUnordered()
                .Select(CheckIndividual)
                .OrderBy(r => r.Name)
                .ToArray();

            var overallStatus = individualResults
                                    .Where(r => r.Status == HealthCheckStatus.NotOkay)
                                    .Any()
                                    ? HealthCheckStatus.NotOkay
                                    : HealthCheckStatus.Okay;

            var message = overallStatus == HealthCheckStatus.Okay
                              ? Messages.AllOkayMessage
                              : Messages.OneOrMoreHealthChecksFailedMessage;

            return new HealthCheckOutcome(overallStatus, message, individualResults);
        }

        private IndividualHealthCheckResult CheckIndividual(IHealthCheck healthCheck)
        {
            var healthCheckName = healthCheck.GetType().Name;

            var sw = Stopwatch.StartNew();
            try
            {
                healthCheck.Check();
                sw.Stop();
                return new SuccessfulIndividualHealthCheckResult(healthCheckName, healthCheck.Description, sw.Elapsed);
            }
            catch (Exception ex)
            {
                sw.Stop();
                var message = string.Format("Health check {0} failed: {1}", healthCheckName, ex.Message);
                _logException(ex, message);
                return new FailedIndividualHealthCheckResult(healthCheckName, healthCheck.Description, ex.Message, sw.Elapsed);
            }
        }
    }
}