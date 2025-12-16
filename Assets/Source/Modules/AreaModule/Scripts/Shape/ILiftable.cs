public interface ILiftable
{
    bool IsRaised { get; }

    void SetStatusRaised();

    void Put();
}