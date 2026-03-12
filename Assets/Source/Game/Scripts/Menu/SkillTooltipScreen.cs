using System;

internal class SkillTooltipScreen : Window
{
    internal event Action ExitButtonClicked;

    protected override void OnButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }
}