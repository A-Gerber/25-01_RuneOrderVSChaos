using UnityEngine;

public class ThirdLightningStrike : UserSkill, ISettableInFirstButton
{
    private const string SkillName = "ThirdLightningStrike";
    private readonly int[,] _configuration;
    private readonly int _offset = -2;

    private string _description;

    public ThirdLightningStrike(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip, int manaCost) : base(iconOnButton, effect, audioClip, manaCost)
    {
        _configuration = new int[,] {
           { 1, 1, 1, 1, 1 },
           { 1, 1, 1, 1, 1 },
           { 1, 1, 1, 1, 1 },
           { 1, 1, 1, 1, 1 },
           { 1, 1, 1, 1, 1 }
       };

        Configuration = _configuration;
        OffsetX = _offset;
        OffsetZ = _offset;
    }

    internal override void PlayEffects(Vector3 position)
    {
        Effect.transform.position = position;
        base.PlayEffects(position);
    }

    internal override void SetDescriptionLanguage(Languages language)
    {
        if (language == Languages.Russian)
        {
            _description = "<color=#FFC300>Удар молнии III\n<color=white>Уничтожает руны в <color=#FFC300>квадрате 5x5";
        }
        else if (language == Languages.Turkish)
        {
            _description = "<color=#FFC300>Yıldırım çarpması III\n5x5 karesindeki<color=white> runeleri yok eder";
        }
        else
        {
            _description = "<color=#FFC300>Lightning strike III\n<color=white>Destroys runes in a <color=#FFC300>square 5x5";
        }

        Description = _description;
    }

    internal override string GetName()
    {
        return SkillName;
    }
}