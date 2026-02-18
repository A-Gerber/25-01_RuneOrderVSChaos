public interface ITakeable
{
    bool IsBusy { get; }
    LocalPosition Position { get; }
    void Take(IReleaseable item);
}