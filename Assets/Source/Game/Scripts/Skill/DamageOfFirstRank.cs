using UnityEngine;

public class DamageOfFirstRank : UserSkill, ISetableInThirdButton
{
    private readonly int _damage = 25;
    private readonly string _description;

    public DamageOfFirstRank(Sprite iconOnButton, ParticleSystem attackZone) : base(iconOnButton, attackZone)
    {
        Damage = _damage;
        _description = "DamageOfFirstRank";
        Description = _description;
    }
}