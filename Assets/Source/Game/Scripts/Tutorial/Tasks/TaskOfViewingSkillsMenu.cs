using System;

internal class TaskOfViewingSkillsMenu : ITask
{
    private readonly ScreenOnViewingSkillsMenu _screen;

    public TaskOfViewingSkillsMenu(ScreenOnViewingSkillsMenu screen)
    {
        _screen = screen != null ? screen : throw new InvalidOperationException("screen is null");

        Subscribe();
    }

    public event Action Completed;

    public void StartTask()
    {
        _screen.Open();
    }

    public void Unsubscribe()
    {
        if (_screen != null)
            _screen.ExitButtonClicked -= OnExitButtonClick;
    }

    private void OnExitButtonClick()
    {
        _screen.Close();
        Completed?.Invoke();
    }

    private void Subscribe()
    {
        if (_screen != null)
            _screen.ExitButtonClicked += OnExitButtonClick;
    }
}
