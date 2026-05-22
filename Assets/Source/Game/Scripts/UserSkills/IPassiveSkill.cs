public interface IPassiveSkill
{
    int DamagePerProjectile { get; }
    int ComboSkillPointsInterval { get; }
    int ComboManaReward { get; }
    float TimeFrameOfCombo { get; }
}