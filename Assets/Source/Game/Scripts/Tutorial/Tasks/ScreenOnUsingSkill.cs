using System;

internal class ScreenOnUsingSkill : Window
{
    internal event Action ExitButtonClicked;

    protected override void OnButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }
}
