using System;
using UnityEngine;

internal class TaskOfOpenSkillMenu : ITask
{
    private readonly ScreenOnOpenSkilslMenu _screen;
    private readonly IReportableOnOpenMenu _gameView;
    private readonly ParticleSystem _arrow;

    private bool _isPerformed = false;

    public TaskOfOpenSkillMenu(ScreenOnOpenSkilslMenu screen, IReportableOnOpenMenu gameView, ParticleSystem arrow)
    {
        _screen = screen != null ? screen : throw new InvalidOperationException("screen is null");
        _gameView = gameView ?? throw new InvalidOperationException("gameView is null");
        _arrow = arrow != null ? arrow : throw new InvalidOperationException("arrow is null");

        Subscribe();
    }

    public event Action Completed;

    public void StartTask()
    {
        _screen.Open();
        _arrow.gameObject.SetActive(true);
        _isPerformed = true;
    }

    public void Unsubscribe()
    {
        if (_gameView != null)
            _gameView.OpenedSkillsMenu -= OnOpenSkillMenu;

        if (_screen != null)
            _screen.ExitButtonClicked -= OnExitButtonClick;

        _arrow.gameObject.SetActive(false);
    }

    private void OnOpenSkillMenu()
    {
        if (_isPerformed)
        {
            _screen.Close();
            _arrow.gameObject.SetActive(false);
            _isPerformed = false;
            Completed?.Invoke();
        }
    }

    private void OnExitButtonClick()
    {
        _screen.Close();
    }

    private void Subscribe()
    {
        if (_gameView != null)
            _gameView.OpenedSkillsMenu += OnOpenSkillMenu;

        if (_screen != null)
            _screen.ExitButtonClicked += OnExitButtonClick;
    }
}
