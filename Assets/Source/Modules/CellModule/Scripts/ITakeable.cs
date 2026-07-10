public interface ITakeable
{
    public bool IsBusy { get; }
    public LocalPosition Position { get; }

    public void Take(IReleasable item);
}