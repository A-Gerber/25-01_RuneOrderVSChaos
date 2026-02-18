using System;

public class Stalactite : IReleaseable
{
    public event Action Released;

    public void Release()
    {       
        Released?.Invoke();
    }

    public void Restart()
    {
        Released?.Invoke();
    }
}