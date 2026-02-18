using System.Collections.Generic;
using UnityEngine;

public class UserSkillFactory : MonoBehaviour
{
    [SerializeField] private UserSkillPerformerView _skillUserViewPrefab;
    [SerializeField] private SkillCardView _skillCardViewPrefab;
    [SerializeField] private Transform _skillContainer;
    [SerializeField] private Transform _effectContainer;
    [SerializeField] private RectTransform _skillViewContainer;
    [SerializeField] private UserSkillView _skillViewPrefab;
    [SerializeField] private UserSkillHandlerView _userSkillHandlerView;
    [SerializeField] private float _heightOfForceImpact = -1f;
    [SerializeField] private float _forceImpact = 5f;
    [SerializeField] private Sprite _emptySprite;

    [Header("LightningStrikes")]
    [SerializeField] private AudioClip _firstLightningStrikeSound;
    [SerializeField] private ParticleSystem _firstLightningStrikeEffect;
    [SerializeField] private Sprite _firstLightningStrikeIcon;
    [SerializeField] private int _firstLightningStrikeThreshold = 1;
    [SerializeField] private ParticleSystem _secondLightningStrikeEffect;
    [SerializeField] private Sprite _secondLightningStrikeIcon;
    [SerializeField] private int _secondLightningStrikeThreshold = 1;
    [SerializeField] private ParticleSystem _thirdLightningStrikeEffect;
    [SerializeField] private Sprite _thirdLightningStrikeIcon;
    [SerializeField] private int _thirdLightningStrikeThreshold = 15;
    [SerializeField] private ParticleSystem _fourthLightningStrikeEffect;
    [SerializeField] private Sprite _fourthLightningStrikeIcon;
    [SerializeField] private int _fourthLightningStrikeThreshold = 45;

    [Header("FillingSkills")]
    [SerializeField] private AudioClip _fillingSound;
    [SerializeField] private ParticleSystem _horizontalFillingEffect;
    [SerializeField] private Sprite _horizontalFillingIcon;
    [SerializeField] private int _horizontalFillingThreshold = 10;
    [SerializeField] private ParticleSystem _crossFillingEffect;
    [SerializeField] private Sprite _crossFillingIcon;
    [SerializeField] private int _crossFillingThreshold = 25;
    [SerializeField] private ParticleSystem _verticalFillingEffect;
    [SerializeField] private Sprite _verticalFillingIcon;
    [SerializeField] private int _verticalFillingThreshold = 35;

    [Header("Damage")]
    [SerializeField] private AudioClip _damageEffectSound;
    [SerializeField] private ParticleSystem _firstDamageEffect;
    [SerializeField] private Sprite _damageOfFirstRankIcon;
    [SerializeField] private int _damageOfFirstRankThreshold = 1;
    [SerializeField] private ParticleSystem _secondDamageEffect;
    [SerializeField] private Sprite _damageOfSecondRankIcon;
    [SerializeField] private int _damageOfSecondRankThreshold = 20;
    [SerializeField] private AudioClip _thirdDamageEffectSound;
    [SerializeField] private ParticleSystem _thirdDamageEffect;
    [SerializeField] private Sprite _damageOfThirdRankIcon;
    [SerializeField] private int _damageOfThirdRankThreshold = 30;

    [Header("PassiveSkills")]
    [SerializeField] private Sprite _passiveSkillOfSecondRankIcon;
    [SerializeField] private int _passiveSkillOfSecondRankThreshold = 5;
    [SerializeField] private Sprite _passiveSkillOfThirdRankIcon;
    [SerializeField] private int _passiveSkillOfThirdRankThreshold = 5;
    [SerializeField] private Sprite _passiveSkillOfFourthRankIcon;
    [SerializeField] private int _passiveSkillOfFourthRankThreshold = 5;

    private UserSkillPerformerView _userSkillPerformerView;

    internal SkillCardDiscoverer SkillCardDiscoverer { get; private set; }

    internal UserSkillPerformer CreateUserSkillPerformer()
    {
        UserSkillPerformer performer = new(new Pusher(_heightOfForceImpact), _forceImpact);
        _userSkillPerformerView = Instantiate(_skillUserViewPrefab, _skillContainer);
        _userSkillPerformerView.Initialize(performer);

        return performer;
    }

    internal UserSkillHandler CreateUserSkillHandler(IConfigurableFromSkillSide attacker)
    {
        PassiveSkillOfFirstRank firstPassiveSkill = new(_emptySprite, _firstDamageEffect, _firstLightningStrikeSound);
        SkillCardDiscoverer = new(CreateSkillCards(CreateSkills()));
        UserSkillHandler userSkillHandler = new(SkillCardDiscoverer, attacker, firstPassiveSkill);
        SkillCardDiscoverer.InitializeSkillCards(userSkillHandler);
        _userSkillHandlerView.Initialize(userSkillHandler);

        return userSkillHandler;
    }

    private List<SkillCard> CreateSkillCards(Dictionary<UserSkill, int> skills)
    {
        List<SkillCard> skillCards = new();

        foreach (var skill in skills)
            skillCards.Add(new SkillCard(skill.Key, skill.Value));

        foreach (var card in skillCards)
            Instantiate(_skillCardViewPrefab, _skillViewContainer).Initialize(card);        

        return skillCards;
    }

    private Dictionary<UserSkill, int> CreateSkills()
    {
        Dictionary<UserSkill, int> skills = new()
        {
            { new FirstLightningStrike(_firstLightningStrikeIcon,Instantiate(_firstLightningStrikeEffect, _effectContainer),_firstLightningStrikeSound), _firstLightningStrikeThreshold },
            { new SecondLightningStrike(_secondLightningStrikeIcon, Instantiate(_secondLightningStrikeEffect, _effectContainer), _firstLightningStrikeSound), _secondLightningStrikeThreshold },
            { new ThirdLightningStrike(_thirdLightningStrikeIcon, Instantiate(_thirdLightningStrikeEffect, _effectContainer), _firstLightningStrikeSound), _thirdLightningStrikeThreshold },
            { new FourthLightningStrike(_fourthLightningStrikeIcon, Instantiate(_fourthLightningStrikeEffect, _effectContainer), _firstLightningStrikeSound), _fourthLightningStrikeThreshold },
            { new HorizontalFilling(_horizontalFillingIcon, Instantiate(_horizontalFillingEffect, _effectContainer), _fillingSound), _horizontalFillingThreshold },
            { new CrossFilling(_crossFillingIcon, Instantiate(_crossFillingEffect, _effectContainer), _fillingSound), _crossFillingThreshold },
            { new VerticalFilling(_verticalFillingIcon,Instantiate(_verticalFillingEffect, _effectContainer), _fillingSound), _verticalFillingThreshold },
            { new DamageOfFirstRank(_damageOfFirstRankIcon, Instantiate(_firstDamageEffect, _effectContainer), _damageEffectSound), _damageOfFirstRankThreshold },
            { new DamageOfSecondRank(_damageOfSecondRankIcon, Instantiate(_secondDamageEffect, _effectContainer), _damageEffectSound), _damageOfSecondRankThreshold },
            { new DamageOfThirdRank(_damageOfThirdRankIcon, Instantiate(_thirdDamageEffect, _effectContainer), _thirdDamageEffectSound), _damageOfThirdRankThreshold },
            { new PassiveSkillOfSecondRank(_passiveSkillOfSecondRankIcon, Instantiate(_firstDamageEffect, _effectContainer), _firstLightningStrikeSound), _passiveSkillOfSecondRankThreshold },
            { new PassiveSkillOfThirdRank(_passiveSkillOfThirdRankIcon, Instantiate(_firstDamageEffect, _effectContainer), _firstLightningStrikeSound), _passiveSkillOfThirdRankThreshold },
            { new PassiveSkillOfFourthRank(_passiveSkillOfFourthRankIcon, Instantiate(_firstDamageEffect, _effectContainer), _firstLightningStrikeSound), _passiveSkillOfFourthRankThreshold }
        };

        foreach (var skill in skills)        
            Instantiate(_skillViewPrefab, _skillContainer).Initialize(skill.Key);       

        return skills;
    }
}