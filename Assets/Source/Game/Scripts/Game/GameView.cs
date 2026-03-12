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
    [SerializeField] private TextMeshProUGUI _gameScore;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _skillMenuButton;
    [SerializeField] private Button _skillsTooltipButton;
    [SerializeField] private ParticleSystem _hintAboutUsingSkill;
    [SerializeField] private SkillButton _firstSkillButton;
    [SerializeField] private SkillButton _secondSkillButton;
    [SerializeField] private SkillButton _thirdSkillButton;

    private WaitForSeconds _waitForAttack;
    private Coroutine _coroutine;
    private GameModel _gameModel;
    private IOpenableMenu _menu;

    public event Action OpenedSkillsMenu;

    private void Awake()
    {
        _waitForAttack = new WaitForSeconds(_delayAttack);
    }

    private void OnEnable()
    {
        if (_gameModel != null)
            SubscribeGameModel();
    }

    private void OnDisable()
    {
        UnsubscribeGameModel();
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
    _gameModel.StartNewGame();

    internal void Initialize(GameModel gameModel, IOpenableMenu menu)
    {
        if (_gameModel != null)
            UnsubscribeGameModel();

        _gameModel = gameModel ?? throw new InvalidOperationException("gameModel is null");
        _menu = menu ?? throw new InvalidOperationException("menu is null");

        SubscribeGameModel();
    }

    private void OnMenuButtonClick()
    {
        _menu.OpenMenu();
    }

    private void OnSkillsTooltipButtonClick()
    {
        _menu.OpenSkillsToolTip();
    }

    private void OnSkillMenuButtonClick()
    {
        Time.timeScale = 0;
        OpenedSkillsMenu?.Invoke();
    }

    private void OnSkillButtonClick(UserSkill skill)
    {
        _gameModel.PressSkillButton(skill);
        _hintAboutUsingSkill.gameObject.SetActive(false);
    }

    private void OnStartNewLevel()
    {
        _textLevel.text = $"Level {_gameModel.CurrentLevel}";
        _scillCount.text = $"{_gameModel.SkillCount}";
        _gameScore.text = $"{_gameModel.GameScore}";

        if(_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void OnChangeCountSkill(int value)
    {
        _scillCount.text = $"{value}";
    }

    private void OnHelp()
    {
        _hintAboutUsingSkill.gameObject.SetActive(true);
    }

    private void OnDisableHint()
    {
        _hintAboutUsingSkill.gameObject.SetActive(false);
    }

    private void OnWaitForDelayAttack(bool isUsedSkill)
    {
        _coroutine = StartCoroutine(AttackOverTime(isUsedSkill));
    }

    private IEnumerator AttackOverTime(bool isUsedSkill)
    {
        yield return _waitForAttack;

        if (isUsedSkill)
            _gameModel.UseSkill();
        else
            _gameModel.ProcessStep();
    }

    private void SubscribeGameModel()
    {
        //SubscribeGameModel();

        _gameModel.StartedNewGame += OnStartNewLevel;
        _gameModel.WentToNextLevel += OnStartNewLevel;
        _gameModel.Waited += OnWaitForDelayAttack;
        _gameModel.SkillCountChanged += OnChangeCountSkill;
        _gameModel.Helped += OnHelp;
        _gameModel.DisabledHint += OnDisableHint;

        _menuButton.onClick.AddListener(OnMenuButtonClick);
        _skillMenuButton.onClick.AddListener(OnSkillMenuButtonClick);
        _skillsTooltipButton.onClick.AddListener(OnSkillsTooltipButtonClick);

        _firstSkillButton.ButtonClicked += OnSkillButtonClick;
        _secondSkillButton.ButtonClicked += OnSkillButtonClick;
        _thirdSkillButton.ButtonClicked += OnSkillButtonClick;
    }

    private void UnsubscribeGameModel()
    {
        //UnsubscribeGameModel();

        _gameModel.StartedNewGame -= OnStartNewLevel;
        _gameModel.WentToNextLevel -= OnStartNewLevel;
        _gameModel.Waited -= OnWaitForDelayAttack;
        _gameModel.SkillCountChanged -= OnChangeCountSkill;
        _gameModel.Helped -= OnHelp;
        _gameModel.DisabledHint -= OnDisableHint;

        _menuButton.onClick.RemoveListener(OnMenuButtonClick);
        _skillMenuButton.onClick.RemoveListener(OnSkillMenuButtonClick);
        _skillsTooltipButton.onClick.RemoveListener(OnSkillsTooltipButtonClick);

        _firstSkillButton.ButtonClicked -= OnSkillButtonClick;
        _secondSkillButton.ButtonClicked -= OnSkillButtonClick;
        _thirdSkillButton.ButtonClicked -= OnSkillButtonClick;
    }
}