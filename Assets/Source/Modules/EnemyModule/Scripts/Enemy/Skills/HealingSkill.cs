using System;

public class HealingSkill : IEnemySkill
{
    private readonly float _percentageOfHealing;

    public HealingSkill(float percentageOfHealing)
    {
        if (percentageOfHealing < 0 && percentageOfHealing > 1)
            throw new ArgumentOutOfRangeException(nameof(percentageOfHealing));

        _percentageOfHealing = percentageOfHealing;
    }

    public int HealingValue { get; private set; }
    public int DisplayedHealingValue { get; private set; }

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