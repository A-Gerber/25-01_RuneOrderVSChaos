using UnityEngine;

public class PassiveSkillOfSecondRank : UserSkill, IPassiveSkill
{
    private readonly int _damagePerProjectile = 1;
    private readonly int _comboSkillPointsInterval = 5;
    private readonly float _timeFrameOfCombo = 6f;
    private readonly string _description;

    public PassiveSkillOfSecondRank(Sprite iconOnButton, ParticleSystem attackZone) : base(iconOnButton, attackZone)
    {
        _description = "PassiveSkillOfSecondRank";
        Description = _description;
    }

    public int DamagePerProjectile => _damagePerProjectile;
    public int ComboSkillPointsInterval => _comboSkillPointsInterval;
    public float TimeFrameOfCombo => _timeFrameOfCombo;
}