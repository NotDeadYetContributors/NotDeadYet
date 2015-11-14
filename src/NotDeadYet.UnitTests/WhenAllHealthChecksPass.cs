using NotDeadYet.HealthChecks;
using NotDeadYet.Results;
using NUnit.Framework;
using Shouldly;

namespace NotDeadYet.UnitTests
{
    public class WhenAllHealthChecksPass : UnitTest
    {
        private HealthCheckOutcome _result;

        public override void When()
        {
            var healthChecker = new HealthCheckerBuilder()
                .WithHealthChecks(() => new[] {new AppPoolIsOnline()})
                .Build();
            _result = healthChecker.Check();
        }

        [Test]
        public void TheOverallResultShouldBeAPass()
        {
            _result.Status.ShouldBe(HealthCheckStatus.Okay);
        }
    }
}