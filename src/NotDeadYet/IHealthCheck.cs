using System;

namespace NotDeadYet
{
    public interface IHealthCheck : IDisposable
    {
        string Description { get; }
        void Check();
    }
}