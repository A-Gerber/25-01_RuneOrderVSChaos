using UnityEngine;

public class SecondLightningStrike : UserSkill, ISetableInFirstButton
{
    private const string SkillName = "SecondLightningStrike";
    private readonly int[,] _configuration;
    private readonly int _offset = -1;

    private string _description;

    public SecondLightningStrike(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip, int manaCost) : base(iconOnButton, effect, audioClip, manaCost)
    {
        _configuration = new int[,] {
                { 1, 1, 1 },
                { 1, 1, 1 },
                { 1, 1, 1 }
            };

        Configuration = _configuration;
        OffsetX = _offset;
        OffsetZ = _offset;
    }

    internal override void Use(Vector3 position)
    {
        Effect.transform.position = position;
        base.Use(position);
    }

    internal override void SetDescriptionLanguage(Languages language)
    {
        if (language == Languages.Russian)
        {
            _description = "<color=#FFC300>Удар молнии II\n<color=white>Уничтожает руны в <color=#FFC300>квадрате 3x3";
        }
        else if (language == Languages.Turkish)
        {
            _description = "<color=#FFC300>Yıldırım çarpması II\n3x3 karesindeki<color=white> runeleri yok eder";
        }
        else
        {
            _description = "<color=#FFC300>Lightning strike II\n<color=white>Destroys runes in a <color=#FFC300>square 3x3";
        }

        Description = _description;
    }

    internal override string GetName()
    {
        return SkillName;
    }
}
