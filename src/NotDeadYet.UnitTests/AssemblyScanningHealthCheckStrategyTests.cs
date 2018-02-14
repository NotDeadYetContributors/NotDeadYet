using NotDeadYet.Configuration;
using NUnit.Framework;
using Shouldly;
using System.Linq;

namespace NotDeadYet.UnitTests
{
    [TestFixture]
    public class AssemblyScanningHealthCheckStrategyTests
    {

        [Test]
        public void CanScanForHealthCheckInstances()
        {
            // Arrange
            var sud = new AssemblyScanningHealthCheckStrategy(typeof(TestHealthCheck).Assembly);

            // Act
            var healthChecks = sud.ScanForHealthChecks();

            // Assert
            healthChecks.Any(c => c is TestHealthCheck).ShouldBe(true);
            healthChecks.Any(c => c is ITestHealthCheck).ShouldBe(false);
            healthChecks.Any(c => c is TestHealthCheckBase).ShouldBe(false);
        }

        [Test]
        public void CanScanForHealthCheckInstancesUsingHealthChecker()
        {
            // Arrange
            var healthChecker = new HealthCheckerBuilder().WithHealthChecksFromAssemblies(typeof(TestHealthCheck).Assembly).Build();

            // Act
            var healthCheckOutcome = healthChecker.Check();

            // Assert
            healthCheckOutcome.Message.ShouldBe("All okay");

        }
    }

    public interface ITestHealthCheck
    {
    }

    public class TestHealthCheck : IHealthCheck
    {
        public string Description { get; } = "Testing";

        public void Check()
        {

        }

        public void Dispose()
        {

        }
    }

    public abstract class TestHealthCheckBase : IHealthCheck
    {
        public string Description { get; }

        public void Check()
        {

        }

        public void Dispose()
        {

        }
    }
}
