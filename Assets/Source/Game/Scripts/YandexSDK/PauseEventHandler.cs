using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class PauseEventHandler : MonoBehaviour
{
    private const string FocusPauseKey = "FocusPause";
    private const string ADVPauseKey = "ADVPause";
    private const string GlobalPauseKey = "GlobalPause";

    private readonly PauseController _pauseController = new();

    private IReadOnlyList<IWindowController> _windows;

    private void OnEnable()
    {
        YG2.onOpenAnyAdv += () => _pauseController.AddPauseSourceKey(ADVPauseKey);
        YG2.onCloseAnyAdv += () => _pauseController.RemovePauseSourceKey(ADVPauseKey);
        YG2.onPauseGame += (inPause) =>
        {
            if (inPause)
                _pauseController.AddPauseSourceKey(GlobalPauseKey);
            else
                _pauseController.RemovePauseSourceKey(GlobalPauseKey);
        };

        YG2.onFocusWindowGame += (inFocus) =>
        {
            if (inFocus)
                _pauseController.RemovePauseSourceKey(FocusPauseKey);
            else
                _pauseController.AddPauseSourceKey(FocusPauseKey);
        };
    }

    private void OnDisable()
    {
        YG2.onOpenAnyAdv -= () => _pauseController.AddPauseSourceKey(ADVPauseKey);
        YG2.onCloseAnyAdv -= () => _pauseController.RemovePauseSourceKey(ADVPauseKey);
        YG2.onPauseGame -= (inPause) =>
        {
            if (inPause)
                _pauseController.AddPauseSourceKey(GlobalPauseKey);
            else
                _pauseController.RemovePauseSourceKey(GlobalPauseKey);
        };

        YG2.onFocusWindowGame -= (inFocus) =>
        {
            if (inFocus)
                _pauseController.RemovePauseSourceKey(FocusPauseKey);
            else
                _pauseController.AddPauseSourceKey(FocusPauseKey);
        };
    }

    internal void Initialize(IReadOnlyList<IWindowController> windows)
    {
        if (windows.Count == 0)
            throw new InvalidOperationException("windows is empty");

        if (_windows != null)
        {
            foreach (var window in _windows)
            {
                window.OpenedWindow -= _pauseController.AddPauseSourceKey;
                window.ClosedWindow -= _pauseController.RemovePauseSourceKey;
            }
        }

        _windows = windows ?? throw new ArgumentNullException("windows is null", nameof(windows));

        foreach (var window in _windows)
        {
            window.OpenedWindow += _pauseController.AddPauseSourceKey;
            window.ClosedWindow += _pauseController.RemovePauseSourceKey;
        }
    }
}