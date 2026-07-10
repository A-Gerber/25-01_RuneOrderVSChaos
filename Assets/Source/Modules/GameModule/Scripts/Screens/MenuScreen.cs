using System;
using UnityEngine;
using UnityEngine.UI;

internal class MenuScreen : Window
{
    [SerializeField] private Button _newGameButton;
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
        _newGameButton.onClick.AddListener(() => NewGameButtonClicked?.Invoke());
        _leaderboardButton.onClick.AddListener(() => LeaderboardButtonClicked?.Invoke());
        _settingsButton.onClick.AddListener(() => SettingsButtonClicked?.Invoke());
        _continueButton.onClick.AddListener(() => ContinueButtonClicked?.Invoke());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _newGameButton.onClick.RemoveListener(() => NewGameButtonClicked?.Invoke());
        _leaderboardButton.onClick.RemoveListener(() => LeaderboardButtonClicked?.Invoke());
        _settingsButton.onClick.RemoveListener(() => SettingsButtonClicked?.Invoke());
        _continueButton.onClick.RemoveListener(() => ContinueButtonClicked?.Invoke());
    }

    internal void SetActiveCloseButton(bool value)
    {     
        ExitButton.interactable = value;
        ExitButton.gameObject.SetActive(value);
    }

    internal void SetInteractableContinueButton(bool value)
    {
        _continueButton.interactable = value;
        _continueButton.gameObject.SetActive(value);
    }

    protected override void OnExitButtonClick()
    {
        BackButtonClicked?.Invoke();
    }
}