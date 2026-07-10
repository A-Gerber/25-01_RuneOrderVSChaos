using System;

public class TaskOfGreeting : ITask
{
    private readonly ScreenOnGreeting _screen;
    private readonly IClosableTutorial _taskHandler;

    public TaskOfGreeting(ScreenOnGreeting greetingScreen, IClosableTutorial taskHandler)
    {
        _screen = greetingScreen != null ? greetingScreen : throw new InvalidOperationException("greetingScreen is null");
        _taskHandler = taskHandler ?? throw new InvalidOperationException("taskHandler is null");

        if (_screen != null)
        {
            _screen.ExitButtonClicked += OnComplete;
            _screen.SkipButtonClicked += OnCloseTutorial;
        }
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
}
