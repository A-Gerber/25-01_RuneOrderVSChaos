using UnityEngine;

public class DamageOfSecondRank : UserSkill, ISetableInThirdButton
{
    private const string SkillName = "DamageOfSecondRank";
    private readonly int _damage = 20;

    private string _description;

    public DamageOfSecondRank(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip, int manaCost) : base(iconOnButton, effect, audioClip, manaCost)
    {
        Damage = _damage;
    }

    internal override void SetDescriptionLanguage(Languages language)
    {
        if (language == Languages.Russian)
        {
            _description = $"<color=#FFC300>Луч света II\n<color=white>Наносит противнику <color=#FFC300> {_damage} урона";
        }
        else if (language == Languages.Turkish)
        {
            _description = $"<color=#FFC300>Işık demeti II\n<color=white>Rakibe <color=#FFC300>{_damage}<color=white> hasar verir";
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