using System;
using System.Collections.Generic;

internal class EnemiesGenerator
{
    private readonly IReadOnlyList<IEnemy> _enemies;

    public EnemiesGenerator(List<IEnemy> enemies)
    {
        _enemies = enemies ?? throw new InvalidOperationException("enemies is null");
    }

    internal IEnemy Generate(int level)
    {
        if (UserUtilities.IsInRangeInt(level,1,1))
            return _enemies[0];
        else if(UserUtilities.IsInRangeInt(level, 2, 2))
            return _enemies[1];
        else if (UserUtilities.IsInRangeInt(level, 3, 3))
            return _enemies[2];
        else if (UserUtilities.IsInRangeInt(level, 4, 4))
            return _enemies[3];
        else if (UserUtilities.IsInRangeInt(level, 5, 5))
            return _enemies[4];
        else if (UserUtilities.IsInRangeInt(level, 6, 6))
            return _enemies[5];
        else if (UserUtilities.IsInRangeInt(level, 7, 7))
            return _enemies[6];
        else if (UserUtilities.IsInRangeInt(level, 8, 8))
            return _enemies[7];
        else if (UserUtilities.IsInRangeInt(level, 9, 9))
            return _enemies[8];
        else
            return _enemies[UnityEngine.Random.Range(0, _enemies.Count)];
    }
}