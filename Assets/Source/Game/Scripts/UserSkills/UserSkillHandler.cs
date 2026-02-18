using System;
using System.Collections.Generic;

internal class UserSkillHandler : IAddableSkill, IChangeableLevel
{
    private const int _startScore = 1;

    private readonly List<UserSkill> _tempSkills = new();
    private readonly SkillCardDiscoverer _skillCardDiscoverer;
    private readonly IPassiveSkill _firstPassiveSkill;
    private readonly int _skillPointsInterval = UserUtilities.SkillPointsInterval;
    private readonly IConfigurableFromSkillSide _attacker;
    private ISettingableSkillButton _gameView;
    private int _level;
    private int _score;

    public UserSkillHandler(SkillCardDiscoverer skillCardDiscoverer, IConfigurableFromSkillSide attacker, IPassiveSkill firstPassiveSkill)
    {
        _attacker = attacker ?? throw new InvalidOperationException("attacker is null");
        _firstPassiveSkill = firstPassiveSkill ?? throw new InvalidOperationException("firstPassiveSkill is null");
        _skillCardDiscoverer = skillCardDiscoverer ?? throw new InvalidOperationException("skillCardDiscoverer is null");

        _score = _startScore;
        _attacker.SetParameters(_firstPassiveSkill.DamagePerProjectile, _firstPassiveSkill.ComboSkillPointsInterval, _firstPassiveSkill.TimeFrameOfCombo);
    }

    internal event Action OpenedSkillsMenu;
    internal event Action<int> ChangedScore;

    public void ChangeLevel(int level)
    {
        if (level < 0)
            throw new ArgumentOutOfRangeException(nameof(level));

        _level = level;
        _skillCardDiscoverer.OpenSkillCards(level);

        if (level % _skillPointsInterval == 0)
        {
            _score += UserUtilities.SkillIncrease;
            ChangedScore?.Invoke(_score);
        }
    }

    public void Reset()
    {
        _score = _startScore + _level / _skillPointsInterval;
        ChangedScore?.Invoke(_score);
        _tempSkills.Clear();

        _attacker.SetParameters(_firstPassiveSkill.DamagePerProjectile, _firstPassiveSkill.ComboSkillPointsInterval, _firstPassiveSkill.TimeFrameOfCombo);
        _gameView.ResetSkillButtons();
        _skillCardDiscoverer.Reset();
        _skillCardDiscoverer.OpenSkillCards(_level);
        _skillCardDiscoverer.SetInteracteble(_score > 0);
    }

    internal void Initialize(ISettingableSkillButton gameView)
    {
        if (_gameView != null)
            _gameView.OpenedSkillsMenu -= OnSkillsButtonClick;

        _gameView = gameView ?? throw new InvalidOperationException("gameView is null");

        _gameView.OpenedSkillsMenu += OnSkillsButtonClick;

        ChangedScore?.Invoke(_score);
    }

    internal void ActivateTempScills()
    {
        if (_tempSkills.Count == 0)
            return;

        foreach (var skill in _tempSkills)
        {          
            switch (skill)
            {
                case ISetableInFirstButton _:
                    _gameView.SetFirstUserSkill(skill);
                    break;

                case ISetableInSecondButton _:
                    _gameView.SetSecondUserSkill(skill);
                    break;

                case ISetableInThirdButton _:
                    _gameView.SetThirdUserSkill(skill);
                    break;

                case IPassiveSkill passiveSkill:
                    _attacker.SetParameters(passiveSkill.DamagePerProjectile, passiveSkill.ComboSkillPointsInterval, passiveSkill.TimeFrameOfCombo);
                    break;

                default:
                    break;
            }
        }

        _tempSkills.Clear();
    }

    public void AddSkillToTempList(SkillCard skillCard)
    {
        if (skillCard == null)
            throw new InvalidOperationException("skillCard is null");

        _tempSkills.Add(skillCard.Skill);
        _score--;
        ChangedScore?.Invoke(_score);

        _skillCardDiscoverer.RemoveFromClosedList(skillCard);
        _skillCardDiscoverer.OpenSkillCards(_level);
        _skillCardDiscoverer.SetInteracteble(_score > 0);
    }

    private void OnSkillsButtonClick()
    {
        OpenedSkillsMenu?.Invoke();
    }
}