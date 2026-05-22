using UnityEngine;

public class PassiveSkillOfThirdRank : UserSkill, IPassiveSkill
{
    private const string SkillName = "ThirdPassiveSkill";
    private readonly int _comboManaReward = 60;
    private readonly int _damagePerProjectile = 1;
    private readonly int _comboSkillPointsInterval = 5;
    private readonly float _timeFrameOfCombo = 6f;

    private string _description;

    public PassiveSkillOfThirdRank(Sprite iconOnButton, ParticleSystem effect, AudioClip audioClip, int manaCost) : base(iconOnButton, effect, audioClip, manaCost)
    {    }

    public int DamagePerProjectile => _damagePerProjectile;
    public int ComboSkillPointsInterval => _comboSkillPointsInterval;
    public int ComboManaReward => _comboManaReward;
    public float TimeFrameOfCombo => _timeFrameOfCombo;

    internal override void SetDescriptionLanguage(Languages language)
    {
        if (language == Languages.Russian)
        {
            _description = $"<color=#FFC300>{_comboManaReward}<color=white> очков маны начисляется за <color=#FFC300>{_comboSkillPointsInterval} " +
                 $"последовательных комбинаций в течении {_timeFrameOfCombo} секунд.";
        }
        else if (language == Languages.Turkish)
        {
            _description = $"<color=#FFC300>{_timeFrameOfCombo} saniye<color=white> içinde <color=#FFC300>{_comboSkillPointsInterval} ardışık kombinasyon<color=white> " +
                $"için <color=#FFC300>{_comboManaReward} mana puanı<color=white> kazanılır";
        }
        else
        {
            _description = $"<color=#FFC300>{_comboManaReward}<color=white> mana points are awarded <color=#FFC300>for {_comboSkillPointsInterval} " +
                $"consecutive combinations within {_timeFrameOfCombo} seconds.";
        }

        Description = _description;
    }

    internal override string GetName()
    {
        return SkillName;
    }
}