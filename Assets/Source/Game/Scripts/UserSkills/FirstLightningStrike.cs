using UnityEngine;

public class FirstLightningStrike : UserSkill, ISetableInFirstButton
{
    private const string SkillName = "FirstLightningStrike";
    private readonly int[,] _configuration;
    private readonly int _offset = -1;

    private string _description;

    public FirstLightningStrike(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip, int manaCost) : base(iconOnButton, effect, audioClip, manaCost)
    {
        _configuration = new int[,] {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 1, 0 }
            };

        Configuration = _configuration;
        OffsetX = _offset;
        OffsetZ = _offset;
    }

    internal override void SetDescriptionLanguage(Languages language)
    {
        if (language == Languages.Russian)
        {
            _description = "<color=#FFC300>Удар молнии I\n<color=white>Уничтожает руны в форме <color=#FFC300>креста 3x3";
        }
        else if (language == Languages.Turkish)
        {
            _description = "<color=#FFC300>Yıldırım çarpması I\n3x3 Haç şekli<color=white> runeleri yok eder";
        }
        else
        {
            _description = "<color=#FFC300>Lightning strike I\n<color=white>Destroys runes in the shape of a <color=#FFC300>cross 3x3";
        }

        Description = _description;
    }

    internal override void Use(Vector3 position)
    {
        Effect.transform.position = position;
        base.Use(position);
    }

    internal override string GetName()
    {
        return SkillName;
    }
}