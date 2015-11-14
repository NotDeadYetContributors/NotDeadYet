using NUnit.Framework;

namespace NotDeadYet.UnitTests
{
    [TestFixture]
    public abstract class UnitTest
    {
        [SetUp]
        public virtual void SetUp()
        {
            When();
        }

        public abstract void When();
    }
}