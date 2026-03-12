using UnityEngine;

public class DamageOfSecondRank : UserSkill, ISetableInThirdButton
{
    private readonly int _damage = 20;
    private readonly string _description;

    public DamageOfSecondRank(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip) : base(iconOnButton, effect, audioClip)
    {
        Damage = _damage;
        _description = $"<color=#FFC300>Light ray III\n<color=white>Deals <color=#FFC300>{_damage} damage <color=white>to the enemy";
        Description = _description;
    }
}