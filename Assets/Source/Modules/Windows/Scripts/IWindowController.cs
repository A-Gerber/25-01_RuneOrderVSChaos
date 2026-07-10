using System;

public interface IWindowController
{
    public event Action<string> OpenedWindow;
    public event Action<string> ClosedWindow;
}
