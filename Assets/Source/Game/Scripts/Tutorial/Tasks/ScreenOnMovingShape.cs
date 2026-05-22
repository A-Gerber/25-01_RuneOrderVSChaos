using System;

internal class ScreenOnMovingShape : Window
{
    internal event Action ExitButtonClicked;

    protected override void OnButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }
}
