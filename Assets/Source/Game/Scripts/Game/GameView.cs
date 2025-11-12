using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RuneOrderVSChaos
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private Factory _factory;
        [SerializeField] private float _delayAttack = 0.35f;
        [SerializeField] private TextMeshProUGUI _textLevel;
        [SerializeField] private WinGameScreen _winGameScreen;
        [SerializeField] private EndGameScreen _endGameScreen;

        private WaitForSeconds _wait;
        private GameModel _gameModel;

        private void Awake()
        {
            _wait = new WaitForSeconds(_delayAttack);
            _gameModel = _factory.Create();
        }

        private void OnEnable()
        {
            _gameModel.Waited += OnWaitForDelayAttack;
            _gameModel.GameOvered += OnGameOver;
            _gameModel.GameWined += OnWinGame;

            _winGameScreen.ContinueButtonClicked += OnContinueButtonClick;
            _endGameScreen.RestartButtonClicked += OnRestartButtonClick;
        }

        private void OnDisable()
        {
            _gameModel.Waited -= OnWaitForDelayAttack;
            _gameModel.GameOvered -= OnGameOver;
            _gameModel.GameWined -= OnWinGame;

            _winGameScreen.ContinueButtonClicked -= OnContinueButtonClick;
            _endGameScreen.RestartButtonClicked -= OnRestartButtonClick;
        }

        private void Start()
        {
            _gameModel.NewGame();
            _textLevel.text = $"Level {_gameModel.Level}";
        }

        private void OnContinueButtonClick()
        {
            Time.timeScale = 1;
            _winGameScreen.Close();
        }

        private void OnRestartButtonClick()
        {
            Time.timeScale = 1;
            _endGameScreen.Close();
            _gameModel.Restart();
        }

        private void OnGameOver()
        {
            Time.timeScale = 0;
            _endGameScreen.Open();
        }

        private void OnWinGame()
        {
            //Time.timeScale = 0;
            _textLevel.text = $"Level {_gameModel.Level}";
            _winGameScreen.Open();
        }

        private void OnWaitForDelayAttack()
        {
            StartCoroutine(AttackOverTime());
        }

        private IEnumerator AttackOverTime()
        {
            yield return _wait;
            _gameModel.Attack();
        }
    }
}