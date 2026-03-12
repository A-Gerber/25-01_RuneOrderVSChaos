using UnityEngine;

public class PassiveSkillOfSecondRank : UserSkill, IPassiveSkill
{
    private readonly int _damagePerProjectile = 1;
    private readonly int _comboSkillPointsInterval = 7;
    private readonly float _timeFrameOfCombo = 6f;
    private readonly string _description;

    public PassiveSkillOfSecondRank(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
    {
        _description = $"A skill point is awarded <color=#FFC300>for {_comboSkillPointsInterval} consecutive combinations within {_timeFrameOfCombo} seconds.";
        Description = _description;

    }

    public int DamagePerProjectile => _damagePerProjectile;
    public int ComboSkillPointsInterval => _comboSkillPointsInterval;
    public float TimeFrameOfCombo => _timeFrameOfCombo;
}
