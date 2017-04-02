using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotDeadYet.UnitTests
{
    using System.Diagnostics.CodeAnalysis;

    using NotDeadYet.Configuration;

    using NUnit.Framework;

    [TestFixture]
    public class AssemblyScanningTests
    {
        [Test]
        public void CanFindHealthcheck()
        {
            // Arrange
            var sud = new AssemblyScanningHealthCheckStrategy(typeof(DummyHealthCheck).Assembly);

            // Act
            var checks = sud.ScanForHealthChecks();

            // Assert
            Assert.True(checks.Any(c => c is DummyHealthCheck));
            Assert.False(checks.Any(c => c is IDummyHealthCheckInterface));
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class DummyHealthCheck : IHealthCheck {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string Description { get; }

        public void Check()
        {
            throw new NotImplementedException();
        }
    }

    public interface IDummyHealthCheckInterface : IHealthCheck
    {
        
    }
}
