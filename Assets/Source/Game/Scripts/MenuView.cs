using System;
using System.Collections;
using UnityEngine;

internal class MenuView : MonoBehaviour
{
    private const int Reward = 1;

    [SerializeField] private WinGameScreen _winGameScreen;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private MenuScreen _menuScreen;
    [SerializeField] private float _pauseDelay = 0.5f;

    private IGame _game;
    private WaitForSeconds _wait;

    private void OnEnable()
    {
        _winGameScreen.NextLevelButtonClicked += OnNextLevelButtonClick;
        _endGameScreen.RestartButtonClicked += OnRestartButtonClick;
        _endGameScreen.RewardButtonClicked += OnRewardButtonClick;

        _menuScreen.NewGameButtonClicked += OnNewGameButtonClick;
        _menuScreen.ContinueButtonClicked += OnContinueButtonClick;
        _menuScreen.ExitButtonClicked += OnExitlButtonClick;
    }

    private void OnDisable()
    {
        _winGameScreen.NextLevelButtonClicked -= OnNextLevelButtonClick;
        _endGameScreen.RestartButtonClicked -= OnRestartButtonClick;

        _menuScreen.NewGameButtonClicked -= OnNewGameButtonClick;
        _menuScreen.ContinueButtonClicked -= OnContinueButtonClick;
        _menuScreen.ExitButtonClicked -= OnExitlButtonClick;
    }

    internal void Initialize(IGame game)
    {
        if (_game != null)
        {
            _game.GameOvered -= OnGameOver;
            _game.GameWined -= OnWinGame;
        }

        _game = game ?? throw new InvalidOperationException("game is null");
        _wait = new WaitForSeconds(_pauseDelay);

        _game.GameOvered += OnGameOver;
        _game.GameWined += OnWinGame;
    }

    internal void OpenMenu()
    {
        Time.timeScale = 0;
        _menuScreen.SetInteractableContinueButton(_game.IsPlaying);
        _menuScreen.Open();
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

    private void OnExitlButtonClick()
    {
        Application.Quit();
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