using System;

public class TaskOfOverview : ITask
{
    private readonly ScreenOfOverview _screen;

    public TaskOfOverview(ScreenOfOverview screen)
    {
        _screen = screen != null ? screen : throw new InvalidOperationException("screen is null");

        if (_screen != null)
            _screen.ExitButtonClicked += OnExitButtonClick;
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
}
