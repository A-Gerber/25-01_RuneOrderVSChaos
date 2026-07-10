using System;
using System.Collections.Generic;

public interface ISkillShapeSpawnerContactable
{
    public event Action<List<LocalPosition>> UsingSkillForShapeSpawner;

    public void RewardWithMana(ManaReward reward);
}