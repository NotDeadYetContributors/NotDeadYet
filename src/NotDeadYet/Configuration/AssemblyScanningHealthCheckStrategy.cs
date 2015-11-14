using System;
using System.Linq;
using System.Reflection;
using ThirdDrawer.Extensions.TypeExtensionMethods;

namespace NotDeadYet.Configuration
{
    internal class AssemblyScanningHealthCheckStrategy
    {
        private readonly Assembly[] _assemblies;

        public AssemblyScanningHealthCheckStrategy(params Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }

        public IHealthCheck[] ScanForHealthChecks()
        {
            return _assemblies
                .SelectMany(a => a.GetExportedTypes())
                .OrderBy(t => t.FullName)
                .Distinct()
                .Where(t => t.IsAssignableTo<IHealthCheck>())
                .Where(t => t.IsInstantiable())
                .Select(InstantiateHealthCheck)
                .ToArray();
        }

        private static IHealthCheck InstantiateHealthCheck(Type healthCheckType)
        {
            return (IHealthCheck) Activator.CreateInstance(healthCheckType);
        }
    }
}