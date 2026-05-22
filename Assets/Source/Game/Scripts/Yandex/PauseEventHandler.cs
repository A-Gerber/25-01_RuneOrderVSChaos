using System;
using UnityEngine;
using YG;

internal class PauseEventHandler : MonoBehaviour
{
    private const string FocusPauseKey = "FocusPause";
    private const string ADVPauseKey = "ADVPause";
    private const string GlobalPauseKey = "GlobalPause";

    private readonly PauseController _pauseController = new();

    private IWindowController _userSkillHandlerView = null;
    private IWindowController _menuView = null;

    private void OnEnable()
    {
        Subscribe();

        YG2.onFocusWindowGame += OnFocus;
        YG2.onOpenAnyAdv += () => _pauseController.AddPauseSourceKey(ADVPauseKey);
        YG2.onCloseAnyAdv += () => _pauseController.RemovePauseSourceKey(ADVPauseKey);
        YG2.onPauseGame += (inPause) =>
        {
            if (inPause)
                _pauseController.AddPauseSourceKey(GlobalPauseKey);
            else
                _pauseController.RemovePauseSourceKey(GlobalPauseKey);
        };
    }

    private void OnDisable()
    {
        Unsubscribe();

        YG2.onFocusWindowGame -= OnFocus;
        YG2.onOpenAnyAdv -= () => _pauseController.AddPauseSourceKey(ADVPauseKey);
        YG2.onCloseAnyAdv -= () => _pauseController.RemovePauseSourceKey(ADVPauseKey);
        YG2.onPauseGame -= (inPause) =>
        {
            if (inPause)
                _pauseController.AddPauseSourceKey(GlobalPauseKey);
            else
                _pauseController.RemovePauseSourceKey(GlobalPauseKey);
        };
    }

    internal void Initialize(IWindowController userSkillHandlerView, IWindowController menuView)
    {
        Unsubscribe();

        _userSkillHandlerView = userSkillHandlerView ?? throw new InvalidOperationException("userSkillHandlerView is null");
        _menuView = menuView ?? throw new InvalidOperationException("menuView is null");

        Subscribe();
    }

    private void OnFocus(bool value)
    {
        if (value)
        {
            _pauseController.SetSoundPlayback(!value);
            _pauseController.RemovePauseSourceKey(FocusPauseKey);
        }
        else
        {
            _pauseController.SetSoundPlayback(!value);
            _pauseController.AddPauseSourceKey(FocusPauseKey);
        }
    }

    private void Subscribe()
    {
        if (_userSkillHandlerView != null)
        {
            _userSkillHandlerView.OpenedWindow += _pauseController.AddPauseSourceKey;
            _userSkillHandlerView.ClosedWindow += _pauseController.RemovePauseSourceKey;
        }

        if (_menuView != null)
        {
            _menuView.OpenedWindow += _pauseController.AddPauseSourceKey;
            _menuView.ClosedWindow += _pauseController.RemovePauseSourceKey;
        }
    }

    private void Unsubscribe()
    {
        if (_userSkillHandlerView != null)
        {
            _userSkillHandlerView.OpenedWindow -= _pauseController.AddPauseSourceKey;
            _userSkillHandlerView.ClosedWindow -= _pauseController.RemovePauseSourceKey;
        }

        if (_menuView != null)
        {
            _menuView.OpenedWindow -= _pauseController.AddPauseSourceKey;
            _menuView.ClosedWindow -= _pauseController.RemovePauseSourceKey;
        }
    }
}
