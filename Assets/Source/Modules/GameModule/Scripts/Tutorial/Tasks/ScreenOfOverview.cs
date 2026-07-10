using System;

public class ScreenOfOverview : Window
{
    internal event Action ExitButtonClicked;

    protected override void OnExitButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }
}