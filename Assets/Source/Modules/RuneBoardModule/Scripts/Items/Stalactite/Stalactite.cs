using System;

public class Stalactite : IReleasable
{
    public event Action Released;

    public bool TryRelease()
    {       
        Released?.Invoke();
        return true;
    }

    public void Restart()
    {
        Released?.Invoke();
    }
}