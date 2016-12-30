using System;
using System.Collections.Generic;
using System.Linq;
using NotDeadYet.HealthChecks;
using NotDeadYet.Results;
using NUnit.Framework;
using Shouldly;
using Moq;

namespace NotDeadYet.UnitTests
{
    public class WhenAHealthCheckIsWarning : UnitTest
    {
        private HealthCheckOutcome _result;

        public override void When()
        {
            var warningMock = new Mock<IHealthCheck>();
            warningMock
                .Setup(c => c.Check())
                .Throws(new HealthCheckWarningException("Mocked Warnings"));

            var healthChecker = new HealthCheckerBuilder()
                .WithHealthChecks(() => new[] { new ApplicationIsRunning(), warningMock.Object })
                .Build();
            
            _result = healthChecker.Check();
        }

        [Test]
        public void TheOverallResultShouldBeAWarning()
        {
            _result.Status.ShouldBe(HealthCheckStatus.Warning);
        }
    }
}
