using System;

internal class ScreenOnMovingShape : Window
{
    internal event Action ExitButtonClicked;

    protected override void OnExitButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }
}