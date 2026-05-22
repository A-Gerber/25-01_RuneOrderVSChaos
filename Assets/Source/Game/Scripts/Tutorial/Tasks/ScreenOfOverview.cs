using System;

internal class ScreenOfOverview : Window
{
    internal event Action ExitButtonClicked;

    protected override void OnButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }
}