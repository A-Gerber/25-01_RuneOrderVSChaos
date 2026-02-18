using System;

public class GroundImpact: IEnemySkill
{
    private int _numberOfUses;

    public GroundImpact(int numberOfUses)
    {
        if (numberOfUses <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfUses));

        _numberOfUses = numberOfUses;
    }
    public int NumberOfUses => _numberOfUses;
}