using System;
using UnityEngine;

public class FreezingSkill: IEnemySkill
{
    private readonly int _numberOfUses;
    private readonly Sprite _icon;

    private string _description;

    public FreezingSkill(int numberOfUses, Sprite icon)
    {
        if (numberOfUses <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfUses));

        _numberOfUses = numberOfUses;
        _icon = icon;

        ChangeSkillDescription(Constants.Language);
    }

    public int NumberOfUses => _numberOfUses;
    public Sprite SkillIcon => _icon;
    public string Description => _description;

    public void ChangeSkillDescription(Languages language)
    {
        if (language == Languages.Russian)
        {
            _description = $"<color=#FFC300>Метель <color=white>- замораживает фигуры из рун в количестве {_numberOfUses} шт. Замороженные руны невозможно уничтожить с первого раза";
        }
        else if (language == Languages.Turkish)
        {
            _description = $"<color=#FFC300>Kar Fırtınası <color=white>- {_numberOfUses} adet rün figürlerini dondurur.  Donmuş rünleri ilk seferde yok etmek mümkün değildir";
        }
        else
        {
            _description = $"<color=#FFC300>Snowstorm <color=white>- freezes rune shapes in the amount of {_numberOfUses} pieces. Frozen runes are not destroyed the first time";
        }
    }
}
