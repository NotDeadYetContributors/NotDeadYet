using System;
using System.Linq;
using System.Reflection;
using NotDeadYet.HealthChecks;

namespace NotDeadYet
{
    public class HealthCheckerBuilder
    {
        private GetHealthChecks _healthChecksFunc;
        private readonly LogError _logError;

        public delegate IHealthCheck[] GetHealthChecks();

        public delegate void LogError(Exception ex, string message);

        public HealthCheckerBuilder()
        {
            _healthChecksFunc = () => new[] {new AppPoolIsOnline()};
            _logError = (ex, message) => Console.WriteLine(message);
        }

        public HealthCheckerBuilder WithHealthChecksFromAssemblies(params Assembly[] assemblies)
        {
            _healthChecksFunc = () => assemblies
                                          .SelectMany(a => a.GetExportedTypes())
                                          .Where(t => typeof (IHealthCheck).IsAssignableFrom(t))
                                          .Select(delegate(Type t)
                                                  {
                                                      try
                                                      {
                                                          return Activator.CreateInstance(t);
                                                      }
                                                      catch (Exception ex)
                                                      {
                                                          var description = string.Format("The {0} health check type could not be instantiated.", t.FullName);
                                                          return new ImmediateFailHealthCheck(description, ex);
                                                      }
                                                  })
                                          .Cast<IHealthCheck>()
                                          .ToArray();

            return this;
        }

        public HealthCheckerBuilder WithHealthChecks(GetHealthChecks healthChecksFunc)
        {
            _healthChecksFunc = healthChecksFunc;
            return this;
        }

        public IHealthChecker Build()
        {
            var healthChecker = new HealthChecker(_healthChecksFunc, _logError);
            return healthChecker;
        }
    }
}