using System;

internal class LeaderboardScreen : Window
{
    internal event Action ExitButtonClicked;

    protected override void OnButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }
}