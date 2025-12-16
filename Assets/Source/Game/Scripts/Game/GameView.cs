using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour, ISettingableSkillButton
{
    [SerializeField] private float _delayAttack = 0.35f;
    [SerializeField] private TextMeshProUGUI _textLevel;
    [SerializeField] private TextMeshProUGUI _scillCount;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _skillsButton;
    [SerializeField] private ParticleSystem _hintAboutUsingSkill;
    [SerializeField] private SkillButton _firstSkillButton;
    [SerializeField] private SkillButton _secondSkillButton;
    [SerializeField] private SkillButton _thirdSkillButton;

    private WaitForSeconds _waitForAttack;
    private GameModel _gameModel;
    private IOpenableGameViewMenu _menu;

    public event Action OpenedSkillsMenu;

    private void Awake()
    {
        _waitForAttack = new WaitForSeconds(_delayAttack);
    }

    private void OnEnable()
    {
        //SubscribeGameModel();
    }

    private void OnDisable()
    {
        //UnsubscribeGameModel();
    }

    public void ResetSkillButtons()
    {
        _firstSkillButton.ResetButton();
        _secondSkillButton.ResetButton();
        _thirdSkillButton.ResetButton();
    }

    public void SetFirstUserSkill(UserSkill skill)
    {
        _firstSkillButton.SetUserSkill(skill);
    }

    public void SetSecondUserSkill(UserSkill skill)
    {
        _secondSkillButton.SetUserSkill(skill);
    }

    public void SetThirdUserSkill(UserSkill skill)
    {
        _thirdSkillButton.SetUserSkill(skill);
    }

    internal void NewGame() =>
    _gameModel.NewGame();

    internal void Initialize(GameModel gameModel, IOpenableGameViewMenu menu)
    {
        if (_gameModel != null)
            UnsubscribeGameModel();

        _gameModel = gameModel ?? throw new InvalidOperationException("gameModel is null");
        _menu = menu ?? throw new InvalidOperationException("menu is null");

        SubscribeGameModel();
    }

    private void OnSettingButtonClick()
    {
        _menu.OpenSettings();
    }

    private void OnMenuButtonClick()
    {
        _menu.OpenMenu();
    }

    private void OnSkillsButtonClick()
    {
        OpenedSkillsMenu?.Invoke();
    }

    private void OnSkillButtonClick(UserSkill skill)
    {
        _gameModel.PressSkillButton(skill);
        _hintAboutUsingSkill.gameObject.SetActive(false);
    }

    private void OnChangeLevel(int value)
    {
        _textLevel.text = $"Level {value}";
    }

    private void OnChangeCountSkill(int value)
    {
        _scillCount.text = $"{value}";
    }

    private void OnHelp()
    {
        _hintAboutUsingSkill.gameObject.SetActive(true);
    }

    private void OnWaitForDelayAttack()
    {
        StartCoroutine(AttackOverTime());
    }

    private IEnumerator AttackOverTime()
    {
        yield return _waitForAttack;
        _gameModel.ProcessStep();
    }

    private void SubscribeGameModel()
    {
        //SubscribeGameModel();

        _gameModel.Waited += OnWaitForDelayAttack;
        _gameModel.ChangedLevel += OnChangeLevel;
        _gameModel.SkillCountChanged += OnChangeCountSkill;
        _gameModel.Helped += OnHelp;

        _settingsButton.onClick.AddListener(OnSettingButtonClick);
        _menuButton.onClick.AddListener(OnMenuButtonClick);
        _skillsButton.onClick.AddListener(OnSkillsButtonClick);

        _firstSkillButton.ButtonClicked += OnSkillButtonClick;
        _secondSkillButton.ButtonClicked += OnSkillButtonClick;
        _thirdSkillButton.ButtonClicked += OnSkillButtonClick;
    }

    private void UnsubscribeGameModel()
    {
        //UnsubscribeGameModel();

        _gameModel.Waited -= OnWaitForDelayAttack;
        _gameModel.ChangedLevel -= OnChangeLevel;
        _gameModel.SkillCountChanged -= OnChangeCountSkill;
        _gameModel.Helped += OnHelp;

        _settingsButton.onClick.RemoveListener(OnSettingButtonClick);
        _menuButton.onClick.RemoveListener(OnMenuButtonClick);
        _skillsButton.onClick.RemoveListener(OnSkillsButtonClick);

        _firstSkillButton.ButtonClicked -= OnSkillButtonClick;
        _secondSkillButton.ButtonClicked -= OnSkillButtonClick;
        _thirdSkillButton.ButtonClicked -= OnSkillButtonClick;
    }
}