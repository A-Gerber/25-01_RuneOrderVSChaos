using System;
using UnityEngine;

public class FreezingSkill: IEnemySkill
{
    private readonly string _description;
    private readonly int _numberOfUses;
    private readonly Sprite _icon;

    public FreezingSkill(int numberOfUses, Sprite icon)
    {
        if (numberOfUses <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfUses));

        _numberOfUses = numberOfUses;
        _icon = icon;

        _description = $"<color=#FFC300>Snowstorm <color=white>- freezes runestones in the amount of {numberOfUses} shapes. Frozen runes are not destroyed the first time.";
    }

    public int NumberOfUses => _numberOfUses;
    public Sprite SkillIcon => _icon;
    public string Description => _description;
}
