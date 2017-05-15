using System;
using System.Collections.Generic;
using NotDeadYet.Results;

namespace NotDeadYet
{
    public interface IHealthCheck : IDisposable
    {
        string Description { get; }
        void Check();

    }

    public interface INestedHealthCheck : IHealthCheck
    {
        List<IndividualHealthCheckResult> ChildrenHealthCheckResults { get; }
    }

}