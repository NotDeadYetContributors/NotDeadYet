using System;
using System.Linq;
using System.Reflection;
using NotDeadYet.HealthChecks;
using ThirdDrawer.Extensions.StringExtensionMethods;
using ThirdDrawer.Extensions.TypeExtensionMethods;

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
            _healthChecksFunc = () => ScanAssembliesForHealthChecks(typeof (ApplicationIsRunning).Assembly);
            _logError = (ex, message) => Console.WriteLine(message);
        }

        public HealthCheckerBuilder WithHealthChecksFromAssemblies(params Assembly[] assemblies)
        {
            var allAssemblies = assemblies.Union(new[] {typeof (HealthCheckerBuilder).Assembly}).ToArray();

            _healthChecksFunc = () => ScanAssembliesForHealthChecks(allAssemblies);

            return this;
        }

        private static IHealthCheck[] ScanAssembliesForHealthChecks(params Assembly[] assemblies)
        {
            return assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => t.IsAssignableTo<IHealthCheck>())
                .Where(t => t.IsInstantiable())
                .Distinct()
                .OrderBy(t => t.FullName)
                .Select(delegate(Type t)
                        {
                            try
                            {
                                return Activator.CreateInstance(t);
                            }
                            catch (Exception ex)
                            {
                                var description = "The {0} health check type could not be instantiated.".FormatWith(t.FullName);
                                return new ImmediateFailHealthCheck(description, ex);
                            }
                        })
                .Cast<IHealthCheck>()
                .ToArray();
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