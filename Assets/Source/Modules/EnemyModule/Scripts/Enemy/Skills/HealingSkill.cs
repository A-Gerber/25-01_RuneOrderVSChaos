using System;
using UnityEngine;

public class HealingSkill : IEnemySkill
{
    private readonly float _percentageOfHealing;
    private readonly Sprite _icon;

    private string _description;

    public HealingSkill(float percentageOfHealing, Sprite icon)
    {
        if (percentageOfHealing < 0 && percentageOfHealing > 1)
            throw new ArgumentOutOfRangeException(nameof(percentageOfHealing));

        _percentageOfHealing = percentageOfHealing;
        _icon = icon;

        ChangeSkillDescription(Constants.Language);
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

    public void ChangeSkillDescription(Languages language)
    {
        if (language == Languages.Russian)
        {
            _description = $"<color=#FFC300>Регенерация зеленокожих <color=white>- восстанавливает {_percentageOfHealing * Constants.PercentageMultiplier} процентов от здоровья";
        }
        else if (language == Languages.Turkish)
        {
            _description = $"<color=#FFC300>Yeşilderililerin yenilenmesi <color=white>-Canlarının yüzde {_percentageOfHealing * Constants.PercentageMultiplier}'unu geri kazandırır";
        }
        else
        {
            _description = $"<color=#FFC300>Regeneration of greenskins <color=white>- restores {_percentageOfHealing * Constants.PercentageMultiplier} percent of health";
        }
    }
}