using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotDeadYet.Results
{
    public class WarningIndividualHealthCheckResult : IndividualHealthCheckResult
    {
        private readonly string _warningReason;

        internal WarningIndividualHealthCheckResult(string name, string description, string warningReason, TimeSpan elapsedTime) : base(name, description, elapsedTime)
        {
            _warningReason = warningReason;
        }

        public override HealthCheckStatus Status => HealthCheckStatus.Warning;

        public string WarningReason => _warningReason;
    }
}
