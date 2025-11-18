using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private Factory _factory;
    [SerializeField] private float _delayAttack = 0.35f;
    [SerializeField] private TextMeshProUGUI _textLevel;
    [SerializeField] private TextMeshProUGUI _scillCount;
    [SerializeField] private WinGameScreen _winGameScreen;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _skillButton;

    private WaitForSeconds _wait;
    private GameModel _gameModel;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delayAttack);
        _gameModel = _factory.CreateGameModel();
    }

    private void OnEnable()
    {
        _gameModel.Waited += OnWaitForDelayAttack;
        _gameModel.GameOvered += OnGameOver;
        _gameModel.GameWined += OnWinGame;
        _gameModel.LevelUpped += OnChangeLevel;
        _gameModel.SkillCountChanged += OnChangeCountSkill;

        _settingsButton.onClick.AddListener(OnSettingButtonClick);
        _menuButton.onClick.AddListener(OnMenuButtonClick);
        _skillButton.onClick.AddListener(OnSkillButtonClick);

        _winGameScreen.NextLevelButtonClicked += OnNextLevelButtonClick;
        _endGameScreen.RestartButtonClicked += OnRestartButtonClick;
    }

    private void OnDisable()
    {
        _gameModel.Waited -= OnWaitForDelayAttack;
        _gameModel.GameOvered -= OnGameOver;
        _gameModel.GameWined -= OnWinGame;
        _gameModel.LevelUpped -= OnChangeLevel;
        _gameModel.SkillCountChanged -= OnChangeCountSkill;

        _settingsButton.onClick.RemoveListener(OnSettingButtonClick);
        _menuButton.onClick.RemoveListener(OnMenuButtonClick);
        _skillButton.onClick.RemoveListener(OnSkillButtonClick);

        _winGameScreen.NextLevelButtonClicked -= OnNextLevelButtonClick;
        _endGameScreen.RestartButtonClicked -= OnRestartButtonClick;
    }

    private void Start()
    {
        _gameModel.NewGame();
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

    private void OnGameOver()
    {
        //Time.timeScale = 0;
        _endGameScreen.Open();
    }

    private void OnWinGame()
    {
        //Time.timeScale = 0;
        _winGameScreen.Open();
    }

    private void OnSettingButtonClick()
    {

    }

    private void OnMenuButtonClick()
    {

    }

    private void OnSkillButtonClick()
    {
        _gameModel.PressSkillButton();
    }

    private void OnChangeLevel(int value)
    {
        _textLevel.text = $"Level {value}";
    }

    private void OnChangeCountSkill(int value)
    {
        _scillCount.text = $"{value}";
    }

    private void OnWaitForDelayAttack()
    {
        StartCoroutine(AttackOverTime());
    }

    private IEnumerator AttackOverTime()
    {
        yield return _wait;
        _gameModel.ProcessStep();
    }
}