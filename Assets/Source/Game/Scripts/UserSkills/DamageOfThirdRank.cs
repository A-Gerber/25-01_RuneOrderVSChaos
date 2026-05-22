using UnityEngine;

public class DamageOfThirdRank : UserSkill, ISetableInThirdButton
{
    private const string SkillName = "DamageOfThirdRank";
    private readonly int _damage = 25;

    private string _description;

    public DamageOfThirdRank(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip, int manaCost) : base(iconOnButton, effect, audioClip, manaCost)
    {
        Damage = _damage;
    }

    internal override void SetDescriptionLanguage(Languages language)
    {
        if (language == Languages.Russian)
        {
            _description = $"<color=#FFC300>Луч света III\n<color=white>Наносит противнику <color=#FFC300> {_damage} урона";
        }
        else if (language == Languages.Turkish)
        {
            _description = $"<color=#FFC300>Işık demeti III\n<color=white>Rakibe <color=#FFC300>{_damage}<color=white> hasar verir";
        }
        else
        {
            _description = $"<color=#FFC300>Light ray III\n<color=white>Deals <color=#FFC300>{_damage} damage <color=white>to the enemy";
        }

        Description = _description;
    }

    internal override string GetName()
    {
        return SkillName;
    }
}