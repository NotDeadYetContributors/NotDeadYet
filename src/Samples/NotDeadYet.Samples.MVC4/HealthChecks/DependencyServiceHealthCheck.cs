using System;
using System.Collections.Generic;
using NotDeadYet.Results;

namespace NotDeadYet.Samples.MVC4.HealthChecks
{
    public class DependencyServiceHealthCheck:INestedHealthCheck
    {
        public void Dispose()
        {
            
        }

        public string Description { get { return "Dependency service check."; } }
        public void Check()
        {
           var rnd  = new Random(10);
            for (var i = 0; i < 10; i++)
            {
                var val = rnd.Next(10);
                if (val < 4)
                {
                    ChildrenHealthCheckResults.Add(new FailedIndividualHealthCheckResult("Fail" + i, "Desc", "Randome fail", new TimeSpan(1)));
                }
                else
                {
                    ChildrenHealthCheckResults.Add(new SuccessfulIndividualHealthCheckResult("Success" + i, "Desc",  new TimeSpan(1)));

                }
            }
        }

        public List<IndividualHealthCheckResult> ChildrenHealthCheckResults { get; } = new List<IndividualHealthCheckResult>();
    }
}