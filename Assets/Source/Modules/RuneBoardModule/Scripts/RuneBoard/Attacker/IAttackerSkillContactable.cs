using System;

public interface IAttackerSkillContactable
{
    public event Action<int> RewardingManaUserSkillPerformer;

    public void SetParameters(int damagePerProjectile, int comboSkillPointsInterval, float timeFrameOfCombo);

    public void DamageWithSkill(int count);
}