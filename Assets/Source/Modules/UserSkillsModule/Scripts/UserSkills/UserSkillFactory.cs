using System;
using System.Collections.Generic;
using UnityEngine;

internal class UserSkillFactory : MonoBehaviour
{
    private const int ManaCostPassiveSkill = 0;

    [SerializeField] private Transform _skillContainer;
    [SerializeField] private Transform _effectContainer;
    [SerializeField] private UserSkillPresenter _skillPresenterPrefab;

    [Header("LightningStrikes")]
    [SerializeField] private AudioClip _firstLightningStrikeSound;
    [SerializeField] private ParticleSystem _firstLightningStrikeEffect;
    [SerializeField] private Sprite _firstLightningStrikeIcon;
    [SerializeField] private int _firstLightningStrikeThreshold = 1;
    [SerializeField] private int _firstLightningStrikeManaCost = 0;
    [SerializeField] private ParticleSystem _secondLightningStrikeEffect;
    [SerializeField] private Sprite _secondLightningStrikeIcon;
    [SerializeField] private int _secondLightningStrikeThreshold = 5;
    [SerializeField] private int _secondLightningStrikeManaCost = 0;
    [SerializeField] private ParticleSystem _thirdLightningStrikeEffect;
    [SerializeField] private Sprite _thirdLightningStrikeIcon;
    [SerializeField] private int _thirdLightningStrikeThreshold = 25;
    [SerializeField] private int _thirdLightningStrikeManaCost = 0;
    [SerializeField] private ParticleSystem _fourthLightningStrikeEffect;
    [SerializeField] private Sprite _fourthLightningStrikeIcon;
    [SerializeField] private int _fourthLightningStrikeThreshold = 45;
    [SerializeField] private int _fourthLightningManaCost = 0;

    [Header("FillingSkills")]
    [SerializeField] private AudioClip _fillingSound;
    [SerializeField] private ParticleSystem _horizontalFillingEffect;
    [SerializeField] private Sprite _horizontalFillingIcon;
    [SerializeField] private int _horizontalFillingThreshold = 2;
    [SerializeField] private int _horizontalFillingManaCost = 5;
    [SerializeField] private ParticleSystem _crossFillingEffect;
    [SerializeField] private Sprite _crossFillingIcon;
    [SerializeField] private int _crossFillingThreshold = 15;
    [SerializeField] private int _crossFillingManaCost = 5;
    [SerializeField] private ParticleSystem _verticalFillingEffect;
    [SerializeField] private Sprite _verticalFillingIcon;
    [SerializeField] private int _verticalFillingThreshold = 25;
    [SerializeField] private int _verticalFillingManaCost = 5;

    [Header("Damage")]
    [SerializeField] private AudioClip _damageEffectSound;
    [SerializeField] private ParticleSystem _firstDamageEffect;
    [SerializeField] private Sprite _damageOfFirstRankIcon;
    [SerializeField] private int _damageOfFirstRankThreshold = 5;
    [SerializeField] private int _damageOfFirstRankManaCost = 10;
    [SerializeField] private ParticleSystem _secondDamageEffect;
    [SerializeField] private Sprite _damageOfSecondRankIcon;
    [SerializeField] private int _damageOfSecondRankThreshold = 20;
    [SerializeField] private int _damageOfSecondRankManaCost = 10;
    [SerializeField] private AudioClip _thirdDamageEffectSound;
    [SerializeField] private ParticleSystem _thirdDamageEffect;
    [SerializeField] private Sprite _damageOfThirdRankIcon;
    [SerializeField] private int _damageOfThirdRankThreshold = 30;
    [SerializeField] private int _damageOfThirdRankManaCost = 10;

    [Header("PassiveSkills")]
    [SerializeField] private Sprite _passiveSkillOfFirstRankIcon;
    [SerializeField] private Sprite _passiveSkillOfSecondRankIcon;
    [SerializeField] private int _passiveSkillOfSecondRankThreshold = 10;
    [SerializeField] private Sprite _passiveSkillOfThirdRankIcon;
    [SerializeField] private int _passiveSkillOfThirdRankThreshold = 20;
    [SerializeField] private Sprite _passiveSkillOfFourthRankIcon;
    [SerializeField] private int _passiveSkillOfFourthRankThreshold = 30;
    [SerializeField] private Sprite _passiveSkillOfFifthRankIcon;
    [SerializeField] private int _passiveSkillOfFifthRankThreshold = 40;

    internal int MinManaCost => Math.Min(Math.Min(_firstLightningStrikeManaCost, _horizontalFillingManaCost), _damageOfFirstRankManaCost);

    internal PassiveSkillOfFirstRank CreateFirstPassiveSkill()
    {
        PassiveSkillOfFirstRank firstPassiveSkill = new (_passiveSkillOfFirstRankIcon, _firstDamageEffect, _firstLightningStrikeSound, ManaCostPassiveSkill);
        firstPassiveSkill.SetDescriptionLanguage(Constants.Language);

        return firstPassiveSkill;
    }

    internal Dictionary<UserSkill, int> Create()
    {
        Dictionary<UserSkill, int> skills = new()
        {
            { new FirstLightningStrike(_firstLightningStrikeIcon,CreateEffect(_firstLightningStrikeEffect),_firstLightningStrikeSound, _firstLightningStrikeManaCost), _firstLightningStrikeThreshold },
            { new SecondLightningStrike(_secondLightningStrikeIcon, CreateEffect(_secondLightningStrikeEffect), _firstLightningStrikeSound,_secondLightningStrikeManaCost), _secondLightningStrikeThreshold },
            { new ThirdLightningStrike(_thirdLightningStrikeIcon, CreateEffect(_thirdLightningStrikeEffect), _firstLightningStrikeSound,_thirdLightningStrikeManaCost), _thirdLightningStrikeThreshold },
            { new FourthLightningStrike(_fourthLightningStrikeIcon, CreateEffect(_fourthLightningStrikeEffect), _firstLightningStrikeSound,_fourthLightningManaCost), _fourthLightningStrikeThreshold },
            { new HorizontalFilling(_horizontalFillingIcon, CreateEffect(_horizontalFillingEffect), _fillingSound,_horizontalFillingManaCost), _horizontalFillingThreshold },
            { new CrossFilling(_crossFillingIcon, CreateEffect(_crossFillingEffect), _fillingSound,_crossFillingManaCost), _crossFillingThreshold },
            { new VerticalFilling(_verticalFillingIcon,CreateEffect(_verticalFillingEffect), _fillingSound,_verticalFillingManaCost), _verticalFillingThreshold },
            { new DamageOfFirstRank(_damageOfFirstRankIcon, CreateEffect(_firstDamageEffect), _damageEffectSound,_damageOfFirstRankManaCost), _damageOfFirstRankThreshold },
            { new DamageOfSecondRank(_damageOfSecondRankIcon, CreateEffect(_secondDamageEffect), _damageEffectSound,_damageOfSecondRankManaCost), _damageOfSecondRankThreshold },
            { new DamageOfThirdRank(_damageOfThirdRankIcon, CreateEffect(_thirdDamageEffect), _thirdDamageEffectSound,_damageOfThirdRankManaCost), _damageOfThirdRankThreshold },
            { new PassiveSkillOfSecondRank(_passiveSkillOfSecondRankIcon, CreateEffect(_firstDamageEffect), _firstLightningStrikeSound, ManaCostPassiveSkill), _passiveSkillOfSecondRankThreshold },
            { new PassiveSkillOfThirdRank(_passiveSkillOfThirdRankIcon, CreateEffect(_firstDamageEffect), _firstLightningStrikeSound, ManaCostPassiveSkill), _passiveSkillOfThirdRankThreshold },
            { new PassiveSkillOfFourthRank(_passiveSkillOfFourthRankIcon, CreateEffect(_firstDamageEffect), _firstLightningStrikeSound, ManaCostPassiveSkill), _passiveSkillOfFourthRankThreshold },
            { new PassiveSkillOfFifthRank(_passiveSkillOfFifthRankIcon, CreateEffect(_firstDamageEffect), _firstLightningStrikeSound, ManaCostPassiveSkill), _passiveSkillOfFifthRankThreshold }
        };

        foreach (var skill in skills)
        {
            skill.Key.SetDescriptionLanguage(Constants.Language);
            Instantiate(_skillPresenterPrefab, _skillContainer).Initialize(skill.Key);
        }

        return skills;
    }

    private ParticleSystem CreateEffect(ParticleSystem prefabEffect)
    {
        return Instantiate(prefabEffect, _effectContainer);
    }
}
