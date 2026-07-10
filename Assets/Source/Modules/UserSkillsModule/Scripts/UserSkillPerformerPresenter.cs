using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UserSkillPerformerPresenter : MonoBehaviour, ISettableButtons
{
    [SerializeField] private SkillButton _firstSkillButton;
    [SerializeField] private SkillButton _secondSkillButton;
    [SerializeField] private SkillButton _thirdSkillButton;
    [SerializeField] private Button _skillMenuButton;
    [SerializeField] private ParticleSystem _attackZonePrefab;
    [SerializeField] private Transform _attackZoneContainer;
    [SerializeField] private float _speed = 25f;
    [SerializeField] private float _delay = 0.35f;

    private UserSkillScreen _userSkillScreen;
    private UserSkillPerformer _userSkillPerformer;
    private Transform _transformAttackZone;
    private UserSkill _skill;
    private WaitForSeconds _wait;
    private Coroutine _coroutine;
    private ParticleSystem _attackZone;

    private bool _isEnableAttackZone = false;
    private bool _isPressedButton = false;

    private void Awake()
    {
        _attackZone = Instantiate(_attackZonePrefab);
        _wait = new WaitForSeconds(_delay);
        _transformAttackZone = _attackZone.transform;
        _attackZone.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _skillMenuButton.onClick.AddListener(() => _userSkillScreen.Open());

        _firstSkillButton.ButtonClicked += OnSkillButtonClick;
        _secondSkillButton.ButtonClicked += OnSkillButtonClick;
        _thirdSkillButton.ButtonClicked += OnSkillButtonClick;
    }

    private void OnDisable()
    {
        _skillMenuButton.onClick.RemoveListener(() => _userSkillScreen.Open());

        _firstSkillButton.ButtonClicked -= OnSkillButtonClick;
        _secondSkillButton.ButtonClicked -= OnSkillButtonClick;
        _thirdSkillButton.ButtonClicked -= OnSkillButtonClick;
    }

    private void Update()
    {
        if (_isEnableAttackZone)
        {
            Vector3 targetPosition = UserUtilities.GetCursorPosition(Constants.CameraHeight);
            targetPosition.y = Constants.CellSize;
            _transformAttackZone.position = Vector3.MoveTowards(_transformAttackZone.position, targetPosition, _speed * Time.deltaTime);
        }
    }

    public void Initialize(UserSkillPerformer userSkillPerformer, UserSkillScreen userSkillScreen)
    {
        if (_userSkillPerformer != null)
            _userSkillPerformer.Started -= () => { if (_coroutine != null) StopCoroutine(_coroutine); };

        _userSkillPerformer = userSkillPerformer ?? throw new ArgumentNullException("skillUser is null", nameof(userSkillPerformer));
        _userSkillScreen = userSkillScreen != null ? userSkillScreen : throw new ArgumentNullException("userSkillScreen is null", nameof(userSkillScreen));

        if (_userSkillPerformer != null)
            _userSkillPerformer.Started += () => { if (_coroutine != null) StopCoroutine(_coroutine); };
    }

    public void Set(UserSkill skill)
    {
        switch (skill)
        {
            case ISettableInFirstButton _:
                _firstSkillButton.SetUserSkill(skill, _userSkillPerformer.ManaCostPerLevel);
                break;

            case ISettableInSecondButton _:
                _secondSkillButton.SetUserSkill(skill, _userSkillPerformer.ManaCostPerLevel);
                break;

            case ISettableInThirdButton _:
                _thirdSkillButton.SetUserSkill(skill, _userSkillPerformer.ManaCostPerLevel);
                break;

            case IPassiveSkill passiveSkill:
                _userSkillPerformer.Set(passiveSkill);
                break;

            default:
                break;
        }
    }

    public void ResetSkillButtons()
    {
        _firstSkillButton.ResetButton();
        _secondSkillButton.ResetButton();
        _thirdSkillButton.ResetButton();
    }

    public void UseSkill()
    {
        if (!_isPressedButton)
            return;

        _isPressedButton = false;
        _isEnableAttackZone = false;
        _attackZone.gameObject.SetActive(false);

        Vector3 targetPosition = UserUtilities.GetCursorPosition(Constants.CameraHeight);

        if (_skill is IPassiveSkill || !UserUtilities.IsLocateInArena(targetPosition) || !_userSkillPerformer.CanSpendMana(_skill.ManaCost))
        {
            _userSkillPerformer.CheckOverGame();
        }
        else
        {
            _userSkillPerformer.UseFirstPartOfSkill(_skill, targetPosition);
            _coroutine = StartCoroutine(UseSkillOverTime());
        }
    }

    private void OnSkillButtonClick(UserSkill skill)
    {
        _skill = skill ?? throw new ArgumentNullException("skill is null", nameof(skill));

        _transformAttackZone.position = UserUtilities.GetCursorPosition(Constants.CameraHeight);
        _isPressedButton = true;
        _isEnableAttackZone = true;
        _attackZone.gameObject.SetActive(true);
        _userSkillPerformer.ChangeStateHint(false);
    }

    private IEnumerator UseSkillOverTime()
    {
        yield return _wait;
        _userSkillPerformer.UseSecondPartOfSkill(_skill);
    }
}
