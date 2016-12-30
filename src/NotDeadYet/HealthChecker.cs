using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NotDeadYet.Configuration;
using NotDeadYet.Results;
using ThirdDrawer.Extensions.CollectionExtensionMethods;
using ThirdDrawer.Extensions.StringExtensionMethods;

namespace NotDeadYet
{
    internal class HealthChecker : IHealthChecker
    {
        private static class Messages
        {
            public const string AllOkayMessage = "All okay";
            public const string OneOrMoreHealthChecksWarningMessage = "One or more health checks warning";
            public const string OneOrMoreHealthChecksFailedMessage = "One or more health checks failed";
            public const string NoHealthCheckersProvidedMessage = "No health checks were provided to the health checker.";
            public const string CannotCreateHealthCheckersMessage = "A catastrophic failure occurred. We can't even load our health checks!";
        }

        private readonly HealthCheckerConfiguration.GetHealthChecks _healthChecksFunc;
        private readonly HealthCheckerConfiguration.LogError _logException;
        private readonly TimeSpan _timeout;

        public HealthChecker(HealthCheckerConfiguration.GetHealthChecks healthChecksFunc, HealthCheckerConfiguration.LogError logException, TimeSpan timeout)
        {
            _healthChecksFunc = healthChecksFunc;
            _logException = logException;
            _timeout = timeout;
        }

        public HealthCheckOutcome Check()
        {
            var now = DateTimeOffset.UtcNow;

            IHealthCheck[] healthChecks;
            try
            {
                healthChecks = _healthChecksFunc();
            }
            catch (Exception ex)
            {
                _logException(ex, Messages.CannotCreateHealthCheckersMessage);
                return new HealthCheckOutcome(HealthCheckStatus.NotOkay, Messages.CannotCreateHealthCheckersMessage, new IndividualHealthCheckResult[0], now);
            }

            if (healthChecks.Length == 0)
            {
                return new HealthCheckOutcome(HealthCheckStatus.NotOkay, Messages.NoHealthCheckersProvidedMessage, new IndividualHealthCheckResult[0], now);
            }

            var individualResults = healthChecks
                .AsParallel()
                .AsUnordered()
                .Select(CheckIndividual)
                .OrderBy(r => r.Name)
                .ToArray();

            healthChecks
                .Do(hc => hc.Dispose())
                .Done();

            if (individualResults.All(r => r.Status == HealthCheckStatus.Okay))
            {
                return new HealthCheckOutcome(HealthCheckStatus.Okay, Messages.AllOkayMessage, individualResults, now);
            }

            //if notOkay otherwise assume warning
            return individualResults.Any(r => r.Status == HealthCheckStatus.NotOkay)
                       ? new HealthCheckOutcome(HealthCheckStatus.NotOkay, Messages.OneOrMoreHealthChecksFailedMessage, individualResults, now)
                       : new HealthCheckOutcome(HealthCheckStatus.Warning, Messages.OneOrMoreHealthChecksWarningMessage, individualResults, now);
        }

        private IndividualHealthCheckResult CheckIndividual(IHealthCheck healthCheck)
        {
            var healthCheckName = healthCheck.GetType().Name;

            var sw = Stopwatch.StartNew();
            var checkTask = Task.Factory.StartNew(() => CheckIndividualInternal(healthCheck, healthCheckName, sw));
            var checkCompletedInTime = Task.WaitAll(new Task[] {checkTask}, _timeout);
            sw.Stop();

            return checkCompletedInTime
                       ? checkTask.Result
                       : new FailedIndividualHealthCheckResult(healthCheckName, healthCheck.Description, "Health check timed out.", sw.Elapsed);
        }

        private IndividualHealthCheckResult CheckIndividualInternal(IHealthCheck healthCheck, string healthCheckName, Stopwatch sw)
        {
            try
            {
                healthCheck.Check();
                return new SuccessfulIndividualHealthCheckResult(healthCheckName, healthCheck.Description, sw.Elapsed);
            }
            catch (HealthCheckWarningException ex)
            {
                var message = "Health check {0} warning: {1}".FormatWith(healthCheckName, ex.Message);
                _logException(ex, message);
                return new WarningIndividualHealthCheckResult(healthCheckName, healthCheck.Description, ex.Message, sw.Elapsed);
            }
            catch (Exception ex)
            {
                var message = "Health check {0} failed: {1}".FormatWith(healthCheckName, ex.Message);
                _logException(ex, message);
                return new FailedIndividualHealthCheckResult(healthCheckName, healthCheck.Description, ex.Message, sw.Elapsed);
            }
        }
    }
}