public interface IPassiveSkill
{
    int DamagePerProjectile { get; }
    int ComboSkillPointsInterval { get; }
    float TimeFrameOfCombo { get; }
}