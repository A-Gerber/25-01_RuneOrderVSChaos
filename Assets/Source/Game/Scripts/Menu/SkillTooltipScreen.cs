using System;

internal class SkillTooltipScreen : Window
{
    public event Action ExitButtonClicked;

    protected override void OnButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }
}
