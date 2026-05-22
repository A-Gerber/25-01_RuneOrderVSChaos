using System;
using System.Collections.Generic;

internal class UserSkillHandler : IAddableSkill, IUserSkillHandler
{
    private readonly List<UserSkill> _tempSkills = new();
    private readonly SkillCardDiscoverer _skillCardDiscoverer;
    private readonly IPassiveSkill _firstPassiveSkill;
    private readonly int _skillPointsInterval = Constants.SkillPointsInterval;
    private readonly IConfigurableFromSkillSide _attacker;
    private readonly ISettableComboManaReward _manaGenerator;

    private const int StartSkillsScore = 2;

    private ISettingableSkillButton _gameView;
    private int _level;
    private int _skillScore;

    public UserSkillHandler(SkillCardDiscoverer skillCardDiscoverer, IConfigurableFromSkillSide attacker, IPassiveSkill firstPassiveSkill, ISettableComboManaReward manaGenerator)
    {
        _attacker = attacker ?? throw new InvalidOperationException("attacker is null");
        _firstPassiveSkill = firstPassiveSkill ?? throw new InvalidOperationException("firstPassiveSkill is null");
        _skillCardDiscoverer = skillCardDiscoverer ?? throw new InvalidOperationException("skillCardDiscoverer is null");
        _manaGenerator = manaGenerator ?? throw new InvalidOperationException("manaGenerator is null");

        _attacker.SetParameters(_firstPassiveSkill.DamagePerProjectile, _firstPassiveSkill.ComboSkillPointsInterval, _firstPassiveSkill.TimeFrameOfCombo);
        _manaGenerator.SetComboManaReward(_firstPassiveSkill.ComboManaReward);
    }

    public event Action<SkillsSavedData> SavedSkills;
    internal event Action OpenedSkillsMenu;
    internal event Action<int> ChangedScore;

    public void StartGame(SkillsSavedData data)
    {
        Reset();

        _skillCardDiscoverer.ActivateSkillCards(data.GetActivatedtSkills());
        ActivateTempScills();
    }

    public SkillsSavedData GetSkillsToSave()
    {
        return new SkillsSavedData(_skillCardDiscoverer.GetActivatedSkills());
    }

    public void ChangeLevel(int level)
    {
        if (level < 0)
            throw new ArgumentOutOfRangeException(nameof(level));

        _level = level;
        _skillCardDiscoverer.OpenSkillCards(level);

        if (level % _skillPointsInterval == 0)
        {
            _skillScore += Constants.SkillCountIncrease;
            ChangedScore?.Invoke(_skillScore);
        }
    }

    public void Reset()
    {
        _skillScore = CalculateSkillScore();
        _tempSkills.Clear();

        _attacker.SetParameters(_firstPassiveSkill.DamagePerProjectile, _firstPassiveSkill.ComboSkillPointsInterval, _firstPassiveSkill.TimeFrameOfCombo);
        _manaGenerator.SetComboManaReward(_firstPassiveSkill.ComboManaReward);
        _gameView.ResetSkillButtons();
        _skillCardDiscoverer.Reset();
        _skillCardDiscoverer.OpenSkillCards(_level);
        _skillCardDiscoverer.SetInteracteble(_skillScore > 0);
    }

    public void AddSkillToTempList(SkillCard skillCard)
    {
        if (skillCard == null)
            throw new InvalidOperationException("skillCard is null");

        if (_skillScore <= 0)
            throw new InvalidOperationException("Missing skill points");

        _tempSkills.Add(skillCard.Skill);
        _skillScore--;
        ChangedScore?.Invoke(_skillScore);

        _skillCardDiscoverer.RemoveFromClosedList(skillCard);
        _skillCardDiscoverer.OpenSkillCards(_level);
        _skillCardDiscoverer.SetInteracteble(_skillScore > 0);
    }

    internal void Initialize(ISettingableSkillButton gameView)
    {
        if (_gameView != null)
            _gameView.OpenedSkillsMenu -= OnSkillsButtonClick;

        _gameView = gameView ?? throw new InvalidOperationException("gameView is null");

        _gameView.OpenedSkillsMenu += OnSkillsButtonClick;

        ChangedScore?.Invoke(_skillScore);
    }

    internal void SaveChanges()
    {
        SavedSkills?.Invoke(new SkillsSavedData(_skillCardDiscoverer.GetActivatedSkills()));
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
                    SetParametrs(passiveSkill);
                    break;

                default:
                    break;
            }
        }

        _tempSkills.Clear();
    }

    private int CalculateSkillScore()
    {
        int score = StartSkillsScore + (_level / _skillPointsInterval) * Constants.SkillCountIncrease;
        ChangedScore?.Invoke(score);

        return score;
    }

    private void SetParametrs(IPassiveSkill passiveSkill)
    {
        _attacker.SetParameters(passiveSkill.DamagePerProjectile, passiveSkill.ComboSkillPointsInterval, passiveSkill.TimeFrameOfCombo);
        _manaGenerator.SetComboManaReward(passiveSkill.ComboManaReward);
    }

    private void OnSkillsButtonClick()
    {
        OpenedSkillsMenu?.Invoke();
    }
}