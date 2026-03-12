using UnityEngine;

public class DamageOfFirstRank : UserSkill, ISetableInThirdButton
{
    private readonly int _damage = 15;
    private readonly string _description;

    public DamageOfFirstRank(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
    {
        Damage = _damage;
        _description = $"<color=#FFC300>Light ray III\n<color=white>Deals <color=#FFC300> {_damage}  damage <color=white>to the enemy";
        Description = _description;
    }
}