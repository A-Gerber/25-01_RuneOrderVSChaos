using System;

internal class WinGameScreen : Window
{
    internal event Action NextLevelButtonClicked;

    internal override void Close()
    {
        WindowGroup.alpha = 0f;
        WindowGroup.blocksRaycasts = false;
        ActionButton.interactable = false;
    }

    internal override void Open()
    {
        WindowGroup.alpha = 1f;
        WindowGroup.blocksRaycasts = true;
        ActionButton.interactable = true;
    }

    protected override void OnButtonClick()
    {
        NextLevelButtonClicked?.Invoke();
    }
}