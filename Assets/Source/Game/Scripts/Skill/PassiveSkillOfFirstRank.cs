using UnityEngine;

public class PassiveSkillOfFirstRank : UserSkill, IPassiveSkill
{
    private readonly int _damagePerProjectile = 1;
    private readonly int _comboSkillPointsInterval = 5;
    private readonly float _timeFrameOfCombo = 4f;
    private readonly string _description;

    public PassiveSkillOfFirstRank(Sprite iconOnButton, ParticleSystem attackZone) : base(iconOnButton, attackZone)
    {
        _description = "PassiveSkillOfFirstRank";
        Description = _description;
    }

    public int DamagePerProjectile => _damagePerProjectile;
    public int ComboSkillPointsInterval => _comboSkillPointsInterval;
    public float TimeFrameOfCombo => _timeFrameOfCombo;
}
