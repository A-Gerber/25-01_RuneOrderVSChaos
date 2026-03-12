using System;
using UnityEngine;

public class HealingSkill : IEnemySkill
{
    private readonly string _description;
    private readonly float _percentageOfHealing;
    private readonly Sprite _icon;

    public HealingSkill(float percentageOfHealing, Sprite icon)
    {
        if (percentageOfHealing < 0 && percentageOfHealing > 1)
            throw new ArgumentOutOfRangeException(nameof(percentageOfHealing));

        _percentageOfHealing = percentageOfHealing;
        _icon = icon;

        _description = $"<color=#FFC300>Regeneration of greenskins <color=white>- restores {_percentageOfHealing * Constants.PercentageMultiplier} percent of health.";
    }

    public int HealingValue { get; private set; }
    public int DisplayedHealingValue { get; private set; }
    public Sprite SkillIcon => _icon;
    public string Description => _description;

    public void SetHealingValue(int health)
    {
        if (health <= 0)
            throw new ArgumentOutOfRangeException(nameof(health));

        HealingValue = (int)(health * _percentageOfHealing);
    }

    public void SetDisplayedHealingValue(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        DisplayedHealingValue = value;
    }
}