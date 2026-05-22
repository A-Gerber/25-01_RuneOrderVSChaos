using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour, ISettingableSkillButton, IReportableOnUsedSkill, IReportableOnOpenMenu
{
    private const int FirstIndex = 0;
    private const int SecondIndex = 1;
    private const int ThirdIndex = 2;

    [SerializeField] private float _delayAttack = 0.35f;
    [SerializeField] private TextMeshProUGUI _textLevel;
    [SerializeField] private TextMeshProUGUI _gameScore;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _skillMenuButton;
    [SerializeField] private Button _skillsTooltipButton;
    [SerializeField] private ParticleSystem _hintAboutUsingSkill;
    [SerializeField] private SkillButton _firstSkillButton;
    [SerializeField] private SkillButton _secondSkillButton;
    [SerializeField] private SkillButton _thirdSkillButton;

    private readonly List<SkillButton> _skillButtons = new();
    private WaitForSeconds _waitForAttack;
    private Coroutine _coroutine;
    private GameModel _gameModel;
    private IOpenableMenu _menu;

    public event Action OpenedSkillsMenu;
    public event Action UsedSkill;

    private void Awake()
    {
        _waitForAttack = new WaitForSeconds(_delayAttack);

        _skillButtons.Add(_firstSkillButton);
        _skillButtons.Add(_secondSkillButton);
        _skillButtons.Add(_thirdSkillButton);
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    public void ResetSkillButtons()
    {
        foreach (var button in _skillButtons)
            button.ResetButton();
    }

    public void SetFirstUserSkill(UserSkill skill)
    {
        _skillButtons[FirstIndex].SetUserSkill(skill);
        _skillButtons[FirstIndex].UpdateData(_gameModel.ManaCostPerLevel);
    }

    public void SetSecondUserSkill(UserSkill skill)
    {
        _skillButtons[SecondIndex].SetUserSkill(skill);
        _skillButtons[SecondIndex].UpdateData(_gameModel.ManaCostPerLevel);
    }

    public void SetThirdUserSkill(UserSkill skill)
    {
        _skillButtons[ThirdIndex].SetUserSkill(skill);
        _skillButtons[ThirdIndex].UpdateData(_gameModel.ManaCostPerLevel);
    }

    internal void Initialize(GameModel gameModel, IOpenableMenu menu)
    {
        if (_gameModel != null)
            Unsubscribe();

        _gameModel = gameModel ?? throw new InvalidOperationException("gameModel is null");
        _menu = menu ?? throw new InvalidOperationException("menu is null");

        Subscribe();
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
        OpenedSkillsMenu?.Invoke();
    }

    private void OnSkillButtonClick(UserSkill skill)
    {
        _gameModel.PressSkillButton(skill);
        _hintAboutUsingSkill.gameObject.SetActive(false);
    }

    private void OnStartNewLevel()
    {
        _textLevel.text = $"{_gameModel.CurrentLevel}";
        _gameScore.text = $"{_gameModel.GameScore}";

        foreach (var button in _skillButtons)
            button.UpdateData(_gameModel.ManaCostPerLevel);

        if (_coroutine != null)
            StopCoroutine(_coroutine);
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
        {
            _gameModel.UseSkill();
            UsedSkill?.Invoke();
        }
        else
        { 
            _gameModel.ProcessStep();
        }
    }

    private void Subscribe()
    {
        if (_gameModel != null)
        {
            _gameModel.StartedGame += OnStartNewLevel;
            _gameModel.WentToNextLevel += OnStartNewLevel;
            _gameModel.Waited += OnWaitForDelayAttack;
            _gameModel.Helped += OnHelp;
            _gameModel.DisabledHint += OnDisableHint;
        }

        _menuButton.onClick.AddListener(OnMenuButtonClick);
        _skillMenuButton.onClick.AddListener(OnSkillMenuButtonClick);
        _skillsTooltipButton.onClick.AddListener(OnSkillsTooltipButtonClick);

        _firstSkillButton.ButtonClicked += OnSkillButtonClick;
        _secondSkillButton.ButtonClicked += OnSkillButtonClick;
        _thirdSkillButton.ButtonClicked += OnSkillButtonClick;
    }

    private void Unsubscribe()
    {
        if (_gameModel != null)
        {
            _gameModel.StartedGame -= OnStartNewLevel;
            _gameModel.WentToNextLevel -= OnStartNewLevel;
            _gameModel.Waited -= OnWaitForDelayAttack;
            _gameModel.Helped -= OnHelp;
            _gameModel.DisabledHint -= OnDisableHint;
        }

        _menuButton.onClick.RemoveListener(OnMenuButtonClick);
        _skillMenuButton.onClick.RemoveListener(OnSkillMenuButtonClick);
        _skillsTooltipButton.onClick.RemoveListener(OnSkillsTooltipButtonClick);

        _firstSkillButton.ButtonClicked -= OnSkillButtonClick;
        _secondSkillButton.ButtonClicked -= OnSkillButtonClick;
        _thirdSkillButton.ButtonClicked -= OnSkillButtonClick;
    }
}