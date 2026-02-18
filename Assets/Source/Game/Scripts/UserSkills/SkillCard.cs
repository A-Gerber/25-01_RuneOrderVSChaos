using System;
using UnityEngine;

public class SkillCard
{
    private readonly UserSkill _skill;
    private readonly int _openingThreshold;
    private IAddableSkill _userSkillHandler;

    public SkillCard(UserSkill skill, int openingThreshold)
    {
        if (openingThreshold < 0)
            throw new ArgumentOutOfRangeException(nameof(openingThreshold));

        _skill = skill ?? throw new InvalidOperationException("skill is null");
        _openingThreshold = openingThreshold;
    }

    internal event Action Opened;
    internal event Action Closed;
    internal event Action<bool> ChangedInteractable;

    internal bool IsOpen { get; private set; } = false;
    internal bool CanChangeInteractivity { get; private set; } = false;
    internal bool IsActive { get; private set; } = false;
    internal int OpeningThreshold => _openingThreshold;
    internal UserSkill Skill => _skill;

    internal void Initialize(IAddableSkill userSkillHandler)
    {
        _userSkillHandler = userSkillHandler ?? throw new InvalidOperationException("userSkillHandler is null");
    }

    internal void Open()
    {
        IsOpen = true;
        Opened?.Invoke();
    }

    internal void Close()
    {
        IsOpen = false;
        Closed?.Invoke();
    }

    internal void Activate()
    {
        _userSkillHandler.AddSkillToTempList(this);
    }

    internal void SetInteracteble(bool value)
    {
        ChangedInteractable?.Invoke(value);
    }

    internal Sprite GetIcon()
    {
        return _skill.IconOnButton;
    }

    internal string GetDescription()
    {
        return _skill.SkillDescription;
    }
}