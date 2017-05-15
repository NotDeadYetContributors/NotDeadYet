using System;
using System.Collections.Generic;

namespace NotDeadYet.Results
{
    public class IndividualHealthCheckResult
    {
        public List<IndividualHealthCheckResult> ChildrenHealthCheckResults { get; set; }

        public IndividualHealthCheckResult()
        {
            
        }

        protected IndividualHealthCheckResult(string name, string description, TimeSpan elapsedTime)
            
        {
            Name = name;
            Description = description;
            ElapsedTime = elapsedTime;
        }

        public virtual HealthCheckStatus Status { get; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TimeSpan ElapsedTime { get; set; }


        public bool IsParent => ChildrenHealthCheckResults?.Count > 0;
    }
}