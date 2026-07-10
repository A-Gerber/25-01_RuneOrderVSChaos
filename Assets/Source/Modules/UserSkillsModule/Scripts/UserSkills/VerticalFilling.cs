using UnityEngine;

public class VerticalFilling : UserSkill, ISettableInSecondButton
{
    private const string SkillName = "VerticalFilling";
    private readonly int[,] _configuration;
    private readonly int _offsetX = -1;
    private readonly int _offsetZ = -7;

    private string _description;

    public VerticalFilling(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip, int manaCost) : base(iconOnButton, effect, audioClip, manaCost)
    {
        _configuration = new int[,] {
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1},
           { 1, 1, 1}
       };

        Configuration = _configuration;
        OffsetX = _offsetX;
        OffsetZ = _offsetZ;
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
            _description = "<color=#FFC300>Создание рун III\n<color=white>Создает руны по форме<color=#FFC300> 3 вертикальных линий";
        }
        else if (language == Languages.Turkish)
        {
            _description = "<color=#FFC300>Runelerin oluşturulması III\n3 dikey çizgi <color=white>şeklinde runeler oluşturur";
        }
        else
        {
            _description = "<color=#FFC300>Creating Runes III\n<color=white>Creates runes in the form of <color=#FFC300>three vertical lines";
        }

        Description = _description;
    }

    internal override string GetName()
    {
        return SkillName;
    }
}