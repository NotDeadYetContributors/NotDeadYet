namespace NotDeadYet
{
    public interface IHealthCheck
    {
        string Description { get; }
        void Check();
    }
}