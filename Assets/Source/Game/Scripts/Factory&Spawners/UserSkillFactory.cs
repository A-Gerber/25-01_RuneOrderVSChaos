using System.Collections.Generic;
using UnityEngine;

public class UserSkillFactory : MonoBehaviour
{
    [SerializeField] private UserSkillPerformerView _skillUserViewPrefab;
    [SerializeField] private SkillCardView _skillCardViewPrefab;
    [SerializeField] private Transform _skillContainer;
    [SerializeField] private RectTransform _skillViewContainer;
    [SerializeField] private SkillView _skillViewPrefab;
    [SerializeField] private UserSkillHandlerView _userSkillHandlerView;
    [SerializeField] private int _skillPointsInterval = 5;
    [SerializeField] private Sprite _emptySprite;

    [Header("LightningStrikes")]
    [SerializeField] private Sprite _firstLightningStrikeIcon;
    [SerializeField] private int _firstLightningStrikeThreshold = 1;
    [SerializeField] private ParticleSystem _firstLightningStrikeAttackZone;
    [SerializeField] private Sprite _secondLightningStrikeIcon;
    [SerializeField] private int _secondLightningStrikeThreshold = 1;
    [SerializeField] private Sprite _thirdLightningStrikeIcon;
    [SerializeField] private int _thirdLightningStrikeThreshold = 15;
    [SerializeField] private Sprite _fourthLightningStrikeIcon;
    [SerializeField] private int _fourthLightningStrikeThreshold = 45;

    [Header("FillingSkills")]
    [SerializeField] private Sprite _horizontalFillingIcon;
    [SerializeField] private int _horizontalFillingThreshold = 10;
    [SerializeField] private Sprite _crossFillingIcon;
    [SerializeField] private int _crossFillingThreshold = 25;
    [SerializeField] private Sprite _verticalFillingIcon;
    [SerializeField] private int _verticalFillingThreshold = 35;

    [Header("Damage")]
    [SerializeField] private Sprite _damageOfFirstRankIcon;
    [SerializeField] private int _damageOfFirstRankThreshold = 1;
    [SerializeField] private Sprite _damageOfSecondRankIcon;
    [SerializeField] private int _damageOfSecondRankThreshold = 20;
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

    internal UserSkillPerformer CreateUserSkillPerformer(float minBorderArea, float maxBorderArea, float cameraHeight)
    {
        UserSkillPerformer performer = new(minBorderArea, maxBorderArea, cameraHeight);
        _userSkillPerformerView = Instantiate(_skillUserViewPrefab, _skillContainer);
        _userSkillPerformerView.Initialize(performer);

        return performer;
    }

    internal UserSkillHandler CreateUserSkillHandler(IConfigurableFromSkillSide attacker)
    {
        PassiveSkillOfFirstRank firstPassiveSkill = new(_emptySprite, _firstLightningStrikeAttackZone);
        SkillCardDiscoverer skillCardDiscoverer = new(CreateSkillCards(CreateSkills()));
        UserSkillHandler userSkillHandler = new(skillCardDiscoverer, _skillPointsInterval, attacker, firstPassiveSkill);
        skillCardDiscoverer.InitializeSkillCards(userSkillHandler);
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
        ParticleSystem attackZone = Instantiate(_firstLightningStrikeAttackZone, _userSkillPerformerView.transform);
        attackZone.gameObject.SetActive(false);

        Dictionary<UserSkill, int> skills = new()
        {
            { new FirstLightningStrike(_firstLightningStrikeIcon,attackZone), _firstLightningStrikeThreshold },
            { new SecondLightningStrike(_secondLightningStrikeIcon, attackZone), _secondLightningStrikeThreshold },
            { new ThirdLightningStrike(_thirdLightningStrikeIcon, attackZone), _thirdLightningStrikeThreshold },
            { new FourthLightningStrike(_fourthLightningStrikeIcon, attackZone), _fourthLightningStrikeThreshold },
            { new HorizontalFilling(_horizontalFillingIcon, attackZone), _horizontalFillingThreshold },
            { new CrossFilling(_crossFillingIcon, attackZone), _crossFillingThreshold },
            { new VerticalFilling(_verticalFillingIcon,attackZone), _verticalFillingThreshold },
            { new DamageOfFirstRank(_damageOfFirstRankIcon, attackZone), _damageOfFirstRankThreshold },
            { new DamageOfSecondRank(_damageOfSecondRankIcon, attackZone), _damageOfSecondRankThreshold },
            { new DamageOfThirdRank(_damageOfThirdRankIcon, attackZone), _damageOfThirdRankThreshold },
            { new PassiveSkillOfSecondRank(_passiveSkillOfSecondRankIcon, attackZone), _passiveSkillOfSecondRankThreshold },
            { new PassiveSkillOfThirdRank(_passiveSkillOfThirdRankIcon, attackZone), _passiveSkillOfThirdRankThreshold },
            { new PassiveSkillOfFourthRank(_passiveSkillOfFourthRankIcon, attackZone), _passiveSkillOfFourthRankThreshold }
        };

        foreach (var skill in skills)        
            Instantiate(_skillViewPrefab, _skillContainer).Initialize(skill.Key);       

        return skills;
    }
}