using NotDeadYet.Results;
using NUnit.Framework;
using Shouldly;

namespace NotDeadYet.UnitTests
{
    public class WhenTheHealthCheckerIsGivenNoHealthChecks : UnitTest
    {
        private HealthCheckOutcome _result;

        public override void When()
        {
            var healthChecker = new HealthCheckerBuilder()
                .WithHealthChecks(() => new IHealthCheck[0])
                .Build();
            _result = healthChecker.Check();
        }

        [Test]
        public void TheOverallResultShouldBeAFail()
        {
            _result.Status.ShouldBe(HealthCheckStatus.NotOkay);
        }
    }
}