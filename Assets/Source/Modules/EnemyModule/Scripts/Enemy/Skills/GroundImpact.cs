using System;
using UnityEngine;

public class GroundImpact: IEnemySkill
{
    private readonly string _description;
    private readonly int _numberOfUses;
    private readonly Sprite _icon;

    public GroundImpact(int numberOfUses, Sprite icon)
    {
        if (numberOfUses <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfUses));

        _numberOfUses = numberOfUses;
        _icon = icon;
        _description = $"<color=#FFC300>Stone spikes <color=white>- creates {numberOfUses} stones in the arena. " +
            $"Stone spikes can only be destroyed with the <color=#0079FF>Lightning Strike <color=white>skill.";
    }

    public int NumberOfUses => _numberOfUses;
    public Sprite SkillIcon => _icon;
    public string Description => _description;
}