using System;
using UnityEngine;
using UnityEngine.UI;

internal class MenuScreen : Window
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    internal event Action NewGameButtonClicked;
    internal event Action ContinueButtonClicked;
    internal event Action LeaderboardButtonClicked;
    internal event Action SettingsButtonClicked;
    internal event Action ExitButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        _continueButton.onClick.AddListener(OnContinueButtonClick);
        _leaderboardButton.onClick.AddListener(OnLeaderboardButtonClick);
        _settingsButton.onClick.AddListener(OnSettingsButtonClick);
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _continueButton.onClick.RemoveListener(OnContinueButtonClick);
        _leaderboardButton.onClick.RemoveListener(OnLeaderboardButtonClick);
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
        _exitButton.onClick.RemoveListener(OnExitButtonClick);
    }

    internal void SetInteractableContinueButton(bool value)
    {
        _continueButton.interactable = value;
    }

    protected override void OnButtonClick()
    {
        NewGameButtonClicked?.Invoke();
    }

    private void OnContinueButtonClick()
    {
        ContinueButtonClicked?.Invoke();
    }

    private void OnLeaderboardButtonClick()
    {
        Debug.Log("OnLeaderboardButtonClick");
        LeaderboardButtonClicked?.Invoke();
    }

    private void OnSettingsButtonClick()
    {
        Debug.Log("OnSettingsButtonClick");
        SettingsButtonClicked?.Invoke();
    }

    private void OnExitButtonClick()
    {
        Debug.Log("OnExitButtonClick");
        ExitButtonClicked?.Invoke();
    }
}