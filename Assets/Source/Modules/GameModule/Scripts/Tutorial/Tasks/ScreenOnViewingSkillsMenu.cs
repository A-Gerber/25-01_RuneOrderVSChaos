using System;

internal class ScreenOnViewingSkillsMenu : Window
{
    internal event Action ExitButtonClicked;

    protected override void OnExitButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }
}