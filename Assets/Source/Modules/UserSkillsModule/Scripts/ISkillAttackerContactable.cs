using System;

public interface ISkillAttackerContactable
{
    public event Action<IPassiveSkill> SettingAttacker;
    public event Action<int> UsingSkillForAttacker;

    public void RewardWithMana(ManaReward reward);
}