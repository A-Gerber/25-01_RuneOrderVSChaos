using System;

internal interface IWindowController
{
    event Action<string> OpenedWindow;
    event Action<string> ClosedWindow;
}
