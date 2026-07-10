using System;
using UnityEngine;

internal class TaskOfOpenSkillMenu : ITask
{
    private readonly ScreenOnOpenSkilslMenu _screen;
    private readonly IReportableOpenEvent _userSkillScreen;
    private readonly ParticleSystem _arrow;

    private bool _isPerformed = false;

    public TaskOfOpenSkillMenu(ScreenOnOpenSkilslMenu screen, IReportableOpenEvent userSkillScreen, ParticleSystem arrow)
    {
        _screen = screen != null ? screen : throw new InvalidOperationException("screen is null");
        _userSkillScreen = userSkillScreen ?? throw new InvalidOperationException("_userSkillScreen is null");
        _arrow = arrow != null ? arrow : throw new InvalidOperationException("arrow is null");

        if (_userSkillScreen != null)
            _userSkillScreen.Opened += OnOpenSkillMenu;

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
        if (_userSkillScreen != null)
            _userSkillScreen.Opened -= OnOpenSkillMenu;

        if (_screen != null)
            _screen.ExitButtonClicked -= () => _screen.Close();

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
}