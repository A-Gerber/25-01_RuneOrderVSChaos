using UnityEngine;

public class CrossFilling : UserSkill, ISettableInSecondButton
{
    private const string SkillName = "CrossFilling";
    private readonly int[,] _configuration;
    private readonly int _offset = -7;

    private string _description;

    public CrossFilling(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip, int manaCost) : base(iconOnButton, effect, audioClip, manaCost)
    {
        _configuration = new int[,] {
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0}
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
            _description = "<color=#FFC300>Создание рун II\n<color=white>Создает руны по <color=#FFC300>форме креста";
        }
        else if (language == Languages.Turkish)
        {
            _description = "<color=#FFC300>Runelerin oluşturulması II\nHaç şeklinde<color=white> rünler oluşturur";
        }
        else
        {
            _description = "<color=#FFC300>Creating Runes II\n<color=white>Creates runes in a <color=#FFC300>cross";
        }

        Description = _description;
    }

    internal override string GetName()
    {
        return SkillName;
    }
}