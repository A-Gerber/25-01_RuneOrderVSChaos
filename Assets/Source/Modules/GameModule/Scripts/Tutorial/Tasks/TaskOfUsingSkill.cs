using System;
using UnityEngine;

internal class TaskOfUsingSkill : ITask
{
    private readonly ScreenOnUsingSkill _screen;
    private readonly IReportableOnUsedSkill _gameView;
    private readonly ParticleSystem _arrow;

    private bool _isPerformed = false;

    public TaskOfUsingSkill(ScreenOnUsingSkill screen, IReportableOnUsedSkill gameView, ParticleSystem arrow)
    {
        _screen = screen != null ? screen : throw new InvalidOperationException("screen is null");
        _gameView = gameView ?? throw new InvalidOperationException("gameView is null");
        _arrow = arrow != null ? arrow : throw new InvalidOperationException("arrow is null");

        if (_gameView != null)
            _gameView.UsedSkill += OnUsingSkill;

        if (_screen != null)
            _screen.ExitButtonClicked += () => _screen.Close();
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
            _gameView.UsedSkill -= OnUsingSkill;

        if (_screen != null)
            _screen.ExitButtonClicked -= () => _screen.Close();

        _arrow.gameObject.SetActive(false);
    }

    private void OnUsingSkill()
    {
        if (_isPerformed)
        {
            _screen.Close();
            _arrow.gameObject.SetActive(false);
            _isPerformed = false;
            Completed?.Invoke();
        }
    }
}