using UnityEngine;

public class PassiveSkillOfThirdRank : UserSkill, IPassiveSkill
{
    private readonly int _damagePerProjectile = 1;
    private readonly int _comboSkillPointsInterval = 4;
    private readonly float _timeFrameOfCombo = 6f;
    private readonly string _description;

    public PassiveSkillOfThirdRank(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
    {
        _description = "PassiveSkillOfThirdRank";
        Description = _description;
    }

    public int DamagePerProjectile => _damagePerProjectile;
    public int ComboSkillPointsInterval => _comboSkillPointsInterval;
    public float TimeFrameOfCombo => _timeFrameOfCombo;
}
