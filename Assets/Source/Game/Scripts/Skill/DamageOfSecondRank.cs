using UnityEngine;

public class DamageOfSecondRank : UserSkill, ISetableInThirdButton
{
    private readonly int _damage = 40;
    private readonly string _description;

    public DamageOfSecondRank(Sprite iconOnButton, ParticleSystem attackZone) : base(iconOnButton, attackZone)
    {
        Damage = _damage;
        _description = "DamageOfSecondRank";
        Description = _description;
    }
}