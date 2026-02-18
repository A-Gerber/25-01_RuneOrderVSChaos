using UnityEngine;

public class DamageOfThirdRank : UserSkill, ISetableInThirdButton
{
    private readonly int _damage = 80;
    private readonly string _description;

    public DamageOfThirdRank(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
    {
        Damage = _damage;
        _description = "DamageOfThirdRank";
        Description = _description;
    }
}