using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using NotDeadYet.AspNetCore;
using NotDeadYet.HealthChecks;
using NotDeadYet.Results;
using NUnit.Framework;
using Shouldly;

namespace NotDeadYet.UnitTests.AspNetCore
{
    public class HealthCheckerTests
    {
        [Test]
        public void ShouldResolveHealthChecker()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddHealthCheck();
            var provider = services.BuildServiceProvider();
            provider.GetService<IHealthChecker>().ShouldNotBeNull();
        }

        [Test]
        public void ShouldResolveHealthCheckerWhenAssemblyMarkerTypeIsSpecified()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddHealthCheck(typeof(HealthCheckerTests));
            var provider = services.BuildServiceProvider();
            provider.GetService<IHealthChecker>().ShouldNotBeNull();
        }

        [Test]
        public void ShouldResolveHealthCheckerWhenSingleAssemblyIsSpecified()
        {
            GetHealthChecker(b => b.WithHealthChecksFromAssemblies(typeof(HealthCheckerTests).Assembly)).ShouldNotBeNull();
        }

        [Test]
        public void ShouldResolveHealthCheckerWhenHealthCheckIsSpecified()
        {
            var healthChecker = GetHealthChecker(b => b.WithHealthChecks(() => new[] { new ApplicationIsRunning() }));

            var result = healthChecker.Check();

            result.Status.ShouldBe(HealthCheckStatus.Okay);
        }

        [Test]
        public void ShouldTimeOutWhenHealthCheckTimeOutIsSpecified()
        {
            IHealthChecker healthChecker = GetHealthChecker(b => b.WithHealthChecks(() => new[] { new SlowRunningHealthCheck(5000) }).WithTimeout(TimeSpan.FromMilliseconds(500)));

            var result = healthChecker.Check();

            result.Status.ShouldBe(HealthCheckStatus.NotOkay);
            result.Results.Length.ShouldBe(1);
            ((FailedIndividualHealthCheckResult)result.Results[0]).FailureReason.ShouldBe("Health check timed out.");
        }

        public static IHealthChecker GetHealthChecker(Action<HealthCheckerBuilder> configureHealthCheckerBuilder)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddHealthCheck(configureHealthCheckerBuilder);
            var provider = services.BuildServiceProvider();
            var healthChecker = provider.GetService<IHealthChecker>();
            return healthChecker;
        }


        public class SlowRunningHealthCheck : IHealthCheck
        {
            private readonly int _millisecondsTimeout;

            public SlowRunningHealthCheck(int millisecondsTimeout)
            {
               _millisecondsTimeout = millisecondsTimeout;
            }

            public SlowRunningHealthCheck()
            {

            }

            public string Description
            {
                get { return "Slow running test."; }
            }

            public void Check()
            {
                Thread.Sleep(_millisecondsTimeout);
            }

            public void Dispose()
            {
                Dispose(true);
            }

            protected virtual void Dispose(bool disposing)
            {
            }
        }

    }
}
