using System;
using System.Collections;
using UnityEngine;

internal class MenuView : MonoBehaviour, IOpenableGameViewMenu
{
    private const int Reward = 2;

    [SerializeField] private WinGameScreen _winGameScreen;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private MenuScreen _menuScreen;
    [SerializeField] private float _pauseDelay = 0.5f;

    private IGame _gameModel;
    private WaitForSeconds _wait;

    private void OnEnable()
    {
        _winGameScreen.NextLevelButtonClicked += OnNextLevelButtonClick;
        _endGameScreen.RestartButtonClicked += OnRestartButtonClick;
        _endGameScreen.RewardButtonClicked += OnRewardButtonClick;

        _menuScreen.NewGameButtonClicked += OnNewGameButtonClick;
        _menuScreen.ContinueButtonClicked += OnContinueButtonClick;
    }

    private void OnDisable()
    {
        _winGameScreen.NextLevelButtonClicked -= OnNextLevelButtonClick;
        _endGameScreen.RestartButtonClicked -= OnRestartButtonClick;

        _menuScreen.NewGameButtonClicked -= OnNewGameButtonClick;
        _menuScreen.ContinueButtonClicked -= OnContinueButtonClick;
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        _menuScreen.SetInteractableContinueButton(_gameModel.IsPlaying);
        _menuScreen.Open();
    }

    public void OpenSettings()
    {

    }

    internal void Initialize(IGame gameModel)
    {
        if (_gameModel != null)
        {
            _gameModel.GameOvered -= OnGameOver;
            _gameModel.GameWined -= OnWinGame;
        }

        _gameModel = gameModel ?? throw new InvalidOperationException("game is null");
        _wait = new WaitForSeconds(_pauseDelay);

        _gameModel.GameOvered += OnGameOver;
        _gameModel.GameWined += OnWinGame;
    }

    private void OnNewGameButtonClick()
    {
        Time.timeScale = 1;
        _gameModel.NewGame();
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
        _gameModel.GoToNextLevel();
    }

    private void OnRestartButtonClick()
    {
        Time.timeScale = 1;
        _endGameScreen.Close();
        _gameModel.Restart();
    }

    private void OnRewardButtonClick()
    {
        Time.timeScale = 1;
        _endGameScreen.Close();
        _gameModel.OnRewardSkillPoints(Reward);
    }

    private void OnGameOver()
    {
        StartCoroutine(PutOnPauseOverTime());
        _endGameScreen.Open();
    }

    private void OnWinGame()
    {
        StartCoroutine(PutOnPauseOverTime());
        _winGameScreen.Open();
    }

    private IEnumerator PutOnPauseOverTime()
    {
        yield return _wait;
        Time.timeScale = 0;
    }
}