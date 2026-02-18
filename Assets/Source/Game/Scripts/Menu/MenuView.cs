using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal class MenuView : MonoBehaviour, IOpenableGameViewMenu
{
    private const int Reward = 2;

    [SerializeField] private WinGameScreen _winGameScreen;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private MenuScreen _menuScreen;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private TextMeshProUGUI _winnerText;
    [SerializeField] private float _delay = 0.5f;

    private IGame _game;
    private ISkillCardDiscoverer _skillCardDiscoverer;
    private WaitForSeconds _wait;
    
    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
    }

    private void OnEnable()
    {
        _winGameScreen.NextLevelButtonClicked += OnNextLevelButtonClick;
        _endGameScreen.RestartButtonClicked += OnRestartButtonClick;
        _endGameScreen.RewardButtonClicked += OnRewardButtonClick;
        _settingsScreen.ExitButtonClicked += CloseMenu;

        _menuScreen.NewGameButtonClicked += OnNewGameButtonClick;
        _menuScreen.ContinueButtonClicked += OnContinueButtonClick;
        _menuScreen.SettingsButtonClicked += OpenSettings;
    }

    private void OnDisable()
    {
        _winGameScreen.NextLevelButtonClicked -= OnNextLevelButtonClick;
        _endGameScreen.RestartButtonClicked -= OnRestartButtonClick;
        _endGameScreen.RewardButtonClicked -= OnRewardButtonClick;
        _settingsScreen.ExitButtonClicked -= CloseMenu;

        _menuScreen.NewGameButtonClicked -= OnNewGameButtonClick;
        _menuScreen.ContinueButtonClicked -= OnContinueButtonClick;
        _menuScreen.SettingsButtonClicked -= OpenSettings;
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        _menuScreen.SetInteractableContinueButton(_game.IsPlaying);
        _menuScreen.Open();
    }

    public void OpenSettings()
    {
        Time.timeScale = 0;
        _settingsScreen.Open();
    }

    internal void Initialize(IGame gameModel, ISkillCardDiscoverer skillCardDiscoverer)
    {
        if (_game != null)
        {
            _game.GameOvered -= OnGameOver;
            _game.GameWined -= OnWinGame;
        }

        _game = gameModel ?? throw new InvalidOperationException("game is null");
        _skillCardDiscoverer = skillCardDiscoverer ?? throw new InvalidOperationException("skillCardDiscoverer is null");

        _game.GameOvered += OnGameOver;
        _game.GameWined += OnWinGame;
    }

    private void CloseMenu()
    {
        _settingsScreen.Close();
    }

    private void OnNewGameButtonClick()
    {
        Time.timeScale = 1;
        _game.NewGame();
        _menuScreen.Close();
    }

    private void OnContinueButtonClick()
    {
        Time.timeScale = 1;
        _menuScreen.Close();
    }

    private void OnNextLevelButtonClick()
    {
        Time.timeScale = 1;
        _winGameScreen.Close();
        _game.GoToNextLevel();
    }

    private void OnRestartButtonClick()
    {
        Time.timeScale = 1;
        _endGameScreen.Close();
        _game.Restart();
    }

    private void OnRewardButtonClick()
    {
        Time.timeScale = 1;
        _endGameScreen.Close();
        _game.OnRewardSkillPoints(Reward);
    }

    private void OnGameOver()
    {
        Time.timeScale = 0;
        _endGameScreen.Open();
    }

    private void OnWinGame(int gameScoreIncrease)
    {
        StartCoroutine(OpenWinScreenOverTime());

        if (_skillCardDiscoverer.TryGetSkillSprites(out List<Sprite> sprites, _game.CurrentLevel + 1))
            _winGameScreen.ShowOpenSkills(sprites);
        else
            _winGameScreen.Hide();

        _winGameScreen.UpdateIncreases(gameScoreIncrease, _game.CurrentLevel);
        _winnerText.gameObject.SetActive(true);
    }

    private IEnumerator OpenWinScreenOverTime()
    {
        yield return _wait;
        yield return _wait;
        _winnerText.gameObject.SetActive(false);
        _winGameScreen.Open();

        yield return _wait;
        Time.timeScale = 0;
    }
}