using System;
using System.Collections.Generic;

internal class UserSkillHandler : IAddableSkill
{
    private readonly List<UserSkill> _tempSkills = new();
    private readonly SkillCardDiscoverer _skillCardDiscoverer;
    private readonly UserSkill _firstPassiveSkill;
    private readonly int _skillPointsInterval = Constants.SkillPointsInterval;
    private readonly ISettableButtons _skillPerformerPresenter;

    private const int StartSkillsScore = 2;

    private int _level;
    private int _skillScore;

    public UserSkillHandler(SkillCardDiscoverer skillCardDiscoverer, UserSkill firstPassiveSkill, ISettableButtons skillPerformerPresenter)
    {
        _firstPassiveSkill = firstPassiveSkill ?? throw new ArgumentNullException("firstPassiveSkill is null", nameof(firstPassiveSkill));
        _skillCardDiscoverer = skillCardDiscoverer ?? throw new ArgumentNullException("skillCardDiscoverer is null", nameof(skillCardDiscoverer));
        _skillPerformerPresenter = skillPerformerPresenter ?? throw new ArgumentNullException("skillPerformerPresenter is null", nameof(skillPerformerPresenter));
    }

    internal event Action<int> ChangedScore;

    public void AddSkillToTempList(SkillCard skillCard)
    {
        if (skillCard == null)
            throw new ArgumentNullException("skillCard is null", nameof(skillCard));

        if (_skillScore <= 0)
            throw new InvalidOperationException("Missing skill points");

        _tempSkills.Add(skillCard.Skill);
        _skillScore--;
        ChangedScore?.Invoke(_skillScore);

        _skillCardDiscoverer.RemoveFromClosedList(skillCard);
        _skillCardDiscoverer.OpenSkillCards(_level);
        _skillCardDiscoverer.SetInteracteble(_skillScore > 0);
    }

    internal void StartGame(List<string> activatedtSkills, int level)
    {
        ChangeLevel(level);
        Reset();

        _skillCardDiscoverer.ActivateSkillCards(activatedtSkills);
        ActivateTempScills();
    }

    public List<string> GetSkillsToSave()
    {
        return _skillCardDiscoverer.GetActivatedSkills();
    }

    private void ChangeLevel(int level)
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

    internal void Reset()
    {
        _skillScore = CalculateSkillScore();
        _tempSkills.Clear();

        _skillPerformerPresenter.Set(_firstPassiveSkill);
        _skillPerformerPresenter.ResetSkillButtons();

        _skillCardDiscoverer.Reset();
        _skillCardDiscoverer.OpenSkillCards(_level);
        _skillCardDiscoverer.SetInteracteble(_skillScore > 0);
    }

    internal void ActivateTempScills()
    {
        if (_tempSkills.Count == 0)
            return;

        foreach (var skill in _tempSkills)
            _skillPerformerPresenter.Set(skill);

        _tempSkills.Clear();
    }

    private int CalculateSkillScore()
    {
        int score = StartSkillsScore + (_level / _skillPointsInterval) * Constants.SkillCountIncrease;
        ChangedScore?.Invoke(score);

        return score;
    }
}