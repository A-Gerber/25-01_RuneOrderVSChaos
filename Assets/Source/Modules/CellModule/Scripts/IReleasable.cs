public interface IReleasable
{
    public bool TryRelease();

    public void Restart();
}