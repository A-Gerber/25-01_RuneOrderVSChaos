using UnityEngine;

public class PassiveSkillOfFifthRank : UserSkill, IPassiveSkill
{
    private readonly int _damagePerProjectile = 2;
    private readonly int _comboSkillPointsInterval = 4;
    private readonly float _timeFrameOfCombo = 6f;
    private readonly string _description;

    public PassiveSkillOfFifthRank(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
    {
        _description = $"A skill point is awarded <color=#FFC300>for {_comboSkillPointsInterval} consecutive combinations within {_timeFrameOfCombo} seconds. Double damage.";
        Description = _description;
    }

    public int DamagePerProjectile => _damagePerProjectile;
    public int ComboSkillPointsInterval => _comboSkillPointsInterval;
    public float TimeFrameOfCombo => _timeFrameOfCombo;
}