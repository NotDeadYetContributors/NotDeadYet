using NUnit.Framework;
using Shouldly;
using System;

namespace NotDeadYet.UnitTests
{
    [TestFixture]
    public class AssemblyVersioningTests
    {
        [Test]
        public void AssemblyVersionIsSet()
        {
            var healthOutcome = new HealthCheckOutcome(Results.HealthCheckStatus.Okay, "Test", new Results.IndividualHealthCheckResult[0], DateTimeOffset.Now);

            healthOutcome.NotDeadYet.ShouldNotBeNullOrEmpty();
        }
    }
}
