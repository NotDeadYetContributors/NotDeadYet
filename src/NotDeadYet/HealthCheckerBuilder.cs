using System;
using System.Linq;
using System.Reflection;
using NotDeadYet.Configuration;
using NotDeadYet.HealthChecks;

namespace NotDeadYet
{
    public class HealthCheckerBuilder
    {
        private HealthCheckerConfiguration.GetHealthChecks _healthChecksFunc = new AssemblyScanningHealthCheckStrategy(typeof (ApplicationIsRunning).Assembly).ScanForHealthChecks;
        private HealthCheckerConfiguration.LogError _logError = new DefaultLoggingStrategy().LogError;
        private TimeSpan _timeout = TimeSpan.FromSeconds(5);

        public HealthCheckerBuilder WithHealthChecksFromAssemblies(params Assembly[] assemblies)
        {
            var allAssemblies = assemblies.Union(new[] {typeof (HealthCheckerBuilder).Assembly}).ToArray();
            _healthChecksFunc = new AssemblyScanningHealthCheckStrategy(allAssemblies).ScanForHealthChecks;

            return this;
        }

        public HealthCheckerBuilder WithHealthChecks(HealthCheckerConfiguration.GetHealthChecks healthChecksFunc)
        {
            _healthChecksFunc = healthChecksFunc;
            return this;
        }

        public HealthCheckerBuilder WithLogger(HealthCheckerConfiguration.LogError logError)
        {
            _logError = logError;
            return this;
        }

        public HealthCheckerBuilder WithTimeout(TimeSpan timeout)
        {
            _timeout = timeout;
            return this;
        }

        public IHealthChecker Build()
        {
            var healthChecker = new HealthChecker(_healthChecksFunc, _logError, _timeout);
            return healthChecker;
        }
    }
}