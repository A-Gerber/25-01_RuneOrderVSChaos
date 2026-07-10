using System;

internal class SkillTooltipScreen : Window
{
    public event Action ExitButtonClicked;

    internal bool IsOpen {  get; private set; } = false;

    public override void Close()
    {
        base.Close();
        IsOpen = false;
    }

    public override void Open()
    {
        base.Open();
        IsOpen = true;
    }

    protected override void OnExitButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }
}