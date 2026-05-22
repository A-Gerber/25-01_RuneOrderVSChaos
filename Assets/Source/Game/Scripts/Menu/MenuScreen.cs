using System;
using UnityEngine;
using UnityEngine.UI;

internal class MenuScreen : Window
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Button _settingsButton;

    internal event Action NewGameButtonClicked;
    internal event Action ContinueButtonClicked;
    internal event Action BackButtonClicked;
    internal event Action LeaderboardButtonClicked;
    internal event Action SettingsButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        _backButton.onClick.AddListener(OnBackButtonClick);
        _leaderboardButton.onClick.AddListener(OnLeaderboardButtonClick);
        _settingsButton.onClick.AddListener(OnSettingsButtonClick);
        _continueButton.onClick.AddListener(OnContinueButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _backButton.onClick.RemoveListener(OnBackButtonClick);
        _leaderboardButton.onClick.RemoveListener(OnLeaderboardButtonClick);
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
        _continueButton.onClick.RemoveListener(OnContinueButtonClick);
    }

    internal void SetActiveCloseButton(bool value)
    {     
        _backButton.interactable = value;
        _backButton.gameObject.SetActive(value);
    }

    internal void SetInteractableContinueButton(bool value)
    {
        _continueButton.interactable = value;
        _continueButton.gameObject.SetActive(value);
    }

    protected override void OnButtonClick()
    {
        NewGameButtonClicked?.Invoke();
    }
    private void OnBackButtonClick()
    {
        BackButtonClicked?.Invoke();
    }

    private void OnContinueButtonClick()
    {
        ContinueButtonClicked?.Invoke();
    }

    private void OnLeaderboardButtonClick()
    {
        LeaderboardButtonClicked?.Invoke();
    }

    private void OnSettingsButtonClick()
    {
        SettingsButtonClicked?.Invoke();
    }
}