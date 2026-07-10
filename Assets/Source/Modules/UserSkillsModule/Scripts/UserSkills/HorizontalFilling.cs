using UnityEngine;

public class HorizontalFilling : UserSkill, ISettableInSecondButton
{
    private const string SkillName = "HorizontalFilling";
    private readonly int[,] _configuration;
    private readonly int _offsetX = -7;
    private readonly int _offsetZ = 0;

    private string _description;

    public HorizontalFilling(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip, int manaCost) : base(iconOnButton, effect, audioClip, manaCost)
    {
        _configuration = new int[,] {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
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
            _description = "<color=#FFC300>Создание рун I\n<color=white>Создает руны по <color=#FFC300>горизонтальной линии";
        }
        else if (language == Languages.Turkish)
        {
            _description = "<color=#FFC300>Runelerin oluşturulması I\nYatay bir çizgide<color=white> runeler oluşturur";
        }
        else
        {
            _description = "<color=#FFC300>Creating Runes I\n<color=white>Creates runes in a <color=#FFC300>horizontal line";
        }

        Description = _description;
    }

    internal override string GetName()
    {
        return SkillName;
    }
}