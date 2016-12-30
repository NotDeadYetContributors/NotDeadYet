using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NotDeadYet.HealthChecks;
using NotDeadYet.Results;
using NUnit.Framework;
using Shouldly;

namespace NotDeadYet.UnitTests
{
    public class WhenAHealthCheckIsNotOkay : UnitTest
    {
        private HealthCheckOutcome _result;

        public override void When()
        {
            var warningMock = new Mock<IHealthCheck>();
            warningMock
                .Setup(c => c.Check())
                .Throws(new HealthCheckWarningException("Mocked Warning"));

            var notOkayMock = new Mock<IHealthCheck>();
            notOkayMock
                .Setup(c => c.Check())
                .Throws(new HealthCheckFailedException("Mocked Not Okay"));

            var healthChecker = new HealthCheckerBuilder()
                .WithHealthChecks(() => new[] { new ApplicationIsRunning(), warningMock.Object, notOkayMock.Object })
                .Build();

            _result = healthChecker.Check();
        }

        [Test]
        public void TheOverallResultShouldBeNotOkay()
        {
            _result.Status.ShouldBe(HealthCheckStatus.NotOkay);
        }
    }
}
