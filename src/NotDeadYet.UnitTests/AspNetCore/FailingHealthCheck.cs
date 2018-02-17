using System;

namespace NotDeadYet.UnitTests.AspNetCore
{
    internal class FailingHealthCheck : IHealthCheck
    {
        public FailingHealthCheck()
        {
        }


        public string Description
        {
            get { return "Failing test."; }
        }

        public void Check()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}