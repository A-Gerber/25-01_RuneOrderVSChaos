using UnityEngine;

public class PassiveSkillOfFirstRank : UserSkill, IPassiveSkill
{
    private readonly int _damagePerProjectile = 1;
    private readonly int _comboSkillPointsInterval = 2;
    private readonly float _timeFrameOfCombo = 4f;
    private readonly string _description;

    public PassiveSkillOfFirstRank(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
    {
        _description = $"A skill point is awarded <color=#FFC300>for {_comboSkillPointsInterval} consecutive combinations within {_timeFrameOfCombo} seconds.";
        Description = _description;
    }

    public int DamagePerProjectile => _damagePerProjectile;
    public int ComboSkillPointsInterval => _comboSkillPointsInterval;
    public float TimeFrameOfCombo => _timeFrameOfCombo;
}
