using UnityEngine;

public class DamageOfFirstRank : UserSkill, ISettableInThirdButton
{
    private const string SkillName = "DamageOfFirstRank";
    private readonly int _damage = 15;

    private string _description;

    public DamageOfFirstRank(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip, int manaCost) : base(iconOnButton, effect, audioClip, manaCost)
    {
        Damage = _damage;
    }

    internal override void SetDescriptionLanguage(Languages language)
    {
        if (language == Languages.Russian)
        {
            _description = $"<color=#FFC300>Луч света I\n<color=white>Наносит противнику <color=#FFC300> {_damage} урона";
        }
        else if (language == Languages.Turkish)
        {
            _description = $"<color=#FFC300>Işık demeti I\n<color=white>Rakibe <color=#FFC300>{_damage}<color=white> hasar verir";
        }
        else
        {
            _description = $"<color=#FFC300>Light ray I\n<color=white>Deals <color=#FFC300> {_damage}  damage <color=white>to the enemy";        
        }

        Description = _description;
    }

    internal override string GetName()
    {
        return SkillName;
    }
}