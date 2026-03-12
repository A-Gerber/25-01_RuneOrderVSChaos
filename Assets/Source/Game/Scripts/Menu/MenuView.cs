using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal class MenuView : MonoBehaviour, IOpenableMenu
{
    [SerializeField] private WinGameScreen _winGameScreen;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private MenuScreen _menuScreen;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private SkillTooltipScreen _skillTooltipScreen;
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
        _settingsScreen.ExitButtonClicked += CloseSettings;
        _skillTooltipScreen.ExitButtonClicked += CloseSkillTooltip;

        _menuScreen.NewGameButtonClicked += OnNewGameButtonClick;
        _menuScreen.ContinueButtonClicked += OnContinueButtonClick;
        _menuScreen.SettingsButtonClicked += OpenSettings;
    }



    private void OnDisable()
    {
        _winGameScreen.NextLevelButtonClicked -= OnNextLevelButtonClick;
        _endGameScreen.RestartButtonClicked -= OnRestartButtonClick;
        _endGameScreen.RewardButtonClicked -= OnRewardButtonClick;
        _settingsScreen.ExitButtonClicked -= CloseSettings;
        _skillTooltipScreen.ExitButtonClicked -= CloseSkillTooltip;

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

    public void OpenSkillsToolTip()
    {
        Time.timeScale = 0;
        _skillTooltipScreen.Open();
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

    private void OpenSettings()
    {
        _settingsScreen.Open();
    }

    private void CloseSettings()
    {
        _settingsScreen.Close();
    }

    private void CloseSkillTooltip()
    {
        Time.timeScale = 1;
        _skillTooltipScreen.Close();
    }

    private void OnNewGameButtonClick()
    {
        Time.timeScale = 1;
        _game.StartNewGame();
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
        _game.OnRewardSkillPoints(Constants.RewardForAdvertising);
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
            _winGameScreen.HideSkills();

        _winGameScreen.UpdateIncreases(gameScoreIncrease, _game.CurrentLevel);
        _winnerText.gameObject.SetActive(true);

        if (_game.CurrentLevel == Constants.LastLevel)
            _winGameScreen.ShowWitch();
        else
            _winGameScreen.HideWitch();
    }

    private IEnumerator OpenWinScreenOverTime()
    {
        yield return _wait;
        yield return _wait;
        _winnerText.gameObject.SetActive(false);
        _winGameScreen.Open();
        Time.timeScale = 0;
    }
}