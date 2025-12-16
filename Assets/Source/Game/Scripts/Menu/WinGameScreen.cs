using System;

internal class WinGameScreen : Window
{
    internal event Action NextLevelButtonClicked;

    protected override void OnButtonClick()
    {
        NextLevelButtonClicked?.Invoke();
    }
}