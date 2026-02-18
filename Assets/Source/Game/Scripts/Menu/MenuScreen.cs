using System;
using UnityEngine;
using UnityEngine.UI;

internal class MenuScreen : Window
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Button _settingsButton;

    internal event Action NewGameButtonClicked;
    internal event Action ContinueButtonClicked;
    internal event Action LeaderboardButtonClicked;
    internal event Action SettingsButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        _continueButton.onClick.AddListener(OnContinueButtonClick);
        _leaderboardButton.onClick.AddListener(OnLeaderboardButtonClick);
        _settingsButton.onClick.AddListener(OnSettingsButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _continueButton.onClick.RemoveListener(OnContinueButtonClick);
        _leaderboardButton.onClick.RemoveListener(OnLeaderboardButtonClick);
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
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
        SettingsButtonClicked?.Invoke();
    }
}