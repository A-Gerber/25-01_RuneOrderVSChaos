using System;

public class FreezingSkill: IEnemySkill
{
    private int _numberOfUses;

    public FreezingSkill(int numberOfUses)
    {
        if (numberOfUses <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfUses));

        _numberOfUses = numberOfUses;
    }

    public int NumberOfUses => _numberOfUses;
}
