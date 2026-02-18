using UnityEngine;

public class PassiveSkillOfFourthRank : UserSkill, IPassiveSkill
{
    private readonly int _damagePerProjectile = 2;
    private readonly int _comboSkillPointsInterval = 4;
    private readonly float _timeFrameOfCombo = 6f;
    private readonly string _description;

    public PassiveSkillOfFourthRank(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
    {
        _description = "PassiveSkillOfFourthRank";
        Description = _description;
    }

    public int DamagePerProjectile => _damagePerProjectile;
    public int ComboSkillPointsInterval => _comboSkillPointsInterval;
    public float TimeFrameOfCombo => _timeFrameOfCombo;
}