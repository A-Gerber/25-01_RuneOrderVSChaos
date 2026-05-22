using System;

internal class TaskOfGreeting : ITask
{
    private readonly ScreenOnGreeting _screen;
    private readonly ICloseableTutorial _taskHandler;

    public TaskOfGreeting(ScreenOnGreeting greetingScreen, ICloseableTutorial taskHandler)
    {
        _screen = greetingScreen != null ? greetingScreen : throw new InvalidOperationException("greetingScreen is null");
        _taskHandler = taskHandler ?? throw new InvalidOperationException("taskHandler is null");

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
        {
            _screen.ExitButtonClicked -= OnComplete;
            _screen.SkipButtonClicked -= OnCloseTutorial;
        }
    }

    private void OnComplete()
    {
        _screen.Close();
        Completed?.Invoke();
    }

    private void OnCloseTutorial()
    {
        _screen.Close();
        _taskHandler.CloseTutorial();
    }

    private void Subscribe()
    {
        if (_screen != null)
        {
            _screen.ExitButtonClicked += OnComplete;
            _screen.SkipButtonClicked += OnCloseTutorial;
        }
    }
}
