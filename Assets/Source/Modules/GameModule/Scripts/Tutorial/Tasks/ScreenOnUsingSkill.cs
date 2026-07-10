using System;

internal class ScreenOnUsingSkill : Window
{
    internal event Action ExitButtonClicked;

    protected override void OnExitButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }
}