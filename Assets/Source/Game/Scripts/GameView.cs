using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private Factory _factory;
    [SerializeField] private MenuView _menu;
    [SerializeField] private float _delayAttack = 0.35f;
    [SerializeField] private TextMeshProUGUI _textLevel;
    [SerializeField] private TextMeshProUGUI _scillCount;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _skillButton;

    private WaitForSeconds _wait;
    private GameModel _gameModel;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delayAttack);
        _gameModel = _factory.CreateGameModel();
        _menu.Initialize(_gameModel);
    }

    private void OnEnable()
    {
        _gameModel.Waited += OnWaitForDelayAttack;
        _gameModel.LevelUpped += OnChangeLevel;
        _gameModel.SkillCountChanged += OnChangeCountSkill;

        _settingsButton.onClick.AddListener(OnSettingButtonClick);
        _menuButton.onClick.AddListener(OnMenuButtonClick);
        _skillButton.onClick.AddListener(OnSkillButtonClick);
    }

    private void OnDisable()
    {
        _gameModel.Waited -= OnWaitForDelayAttack;
        _gameModel.LevelUpped -= OnChangeLevel;
        _gameModel.SkillCountChanged -= OnChangeCountSkill;

        _settingsButton.onClick.RemoveListener(OnSettingButtonClick);
        _menuButton.onClick.RemoveListener(OnMenuButtonClick);
        _skillButton.onClick.RemoveListener(OnSkillButtonClick);
    }

    private void Start()
    {
        _gameModel.NewGame();
        //_menu.OpenMenu();
    }

    private void OnSettingButtonClick()
    {

    }

    private void OnMenuButtonClick()
    {
        _menu.OpenMenu();
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
