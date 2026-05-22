using UnityEngine;

public class FourthLightningStrike : UserSkill, ISetableInFirstButton
{
    private const string SkillName = "FourthLightningStrike";
    private readonly int[,] _configuration;
    private readonly int _offset = -3;

    private string _description;

    public FourthLightningStrike(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip, int manaCost) : base(iconOnButton, effect, audioClip, manaCost)
    {
        _configuration = new int[,] {
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 }
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
            _description = "<color=#FFC300>Удар молнии IV\n<color=white>Уничтожает руны в <color=#FFC300>квадрате 7x7";
        }
        else if (language == Languages.Turkish)
        {
            _description = "<color=#FFC300>Yıldırım çarpması IV\n7x7 karesindeki<color=white> runeleri yok eder";
        }
        else
        {
            _description = "<color=#FFC300>Lightning strike IV\n<color=white>Destroys runes in a <color=#FFC300>square 7x7";
        }

        Description = _description;
    }

    internal override string GetName()
    {
        return SkillName;
    }
}