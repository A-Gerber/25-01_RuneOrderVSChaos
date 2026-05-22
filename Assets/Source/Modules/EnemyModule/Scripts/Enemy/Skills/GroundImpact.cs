using System;
using UnityEngine;

public class GroundImpact: IEnemySkill
{
    private readonly int _numberOfUses;
    private readonly Sprite _icon;

    private string _description;

    public GroundImpact(int numberOfUses, Sprite icon)
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
            _description = $"<color=#FFC300>Каменные шипы <color=white>- создает на арене камни в количестве {_numberOfUses} шт. " +
                            $"Каменные шипы можно уничтожить тодько с помощью навыка <color=#0079FF>Удар молнии";
        }
        else if (language == Languages.Turkish)
        {
            _description = $"<color=#FFC300>Taş Sivri Uçlar <color=white>- Arenaya {_numberOfUses} adet taş yerleştirir. Taş sivri uçlar yalnızca Yıldırım Çarpması becerisiyle yok edilebilir";
        }
        else
        {
            _description = $"<color=#FFC300>Stone spikes <color=white>- creates {_numberOfUses} stones in the arena. " +
                            $"Stone spikes can only be destroyed with the <color=#0079FF>Lightning Strike <color=white>skill";
        }
    }
}