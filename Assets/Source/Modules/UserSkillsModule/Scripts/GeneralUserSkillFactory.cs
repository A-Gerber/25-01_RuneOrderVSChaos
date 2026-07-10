using System.Collections.Generic;
using UnityEngine;

public class GeneralUserSkillFactory : MonoBehaviour
{
    [SerializeField] private UserSkillFactory _userSkillFactory;
    [SerializeField] private SkillCardsFactory _skillCardFactory;
    [SerializeField] private UserSkillPerformerPresenter _performerPresenter;
    [SerializeField] private UserSkillScreen _userSkillScreen;
    [SerializeField] private ManaGeneratorPresenter _manaGeneratorPresenterPrefab;
    [SerializeField] private RectTransform _container;
    [SerializeField] private ParticleSystem _hintAboutUsingSkill;
    [SerializeField] private int _manaPerCube = 1;

    private List<SkillCardPresenter> _skillCardsForLanguageHandler;
    private Dictionary<UserSkill, int> _skillsWithThreshold;

    public IShowableNextSkills SkillCardDiscoverer {  get; private set; }
    public UserSkillScreen UserSkillScreen => _userSkillScreen;
    public UserSkillPerformerPresenter UserSkillPerformerPresenter => _performerPresenter;

    public UserSkillPerformer Create(IIdentifiableTargets mediator)
    {
        PassiveSkillOfFirstRank firstPassiveSkill = _userSkillFactory.CreateFirstPassiveSkill();

        _skillsWithThreshold = _userSkillFactory.Create();
        List<SkillCard> skillCards = _skillCardFactory.Create(_skillsWithThreshold);
        _skillCardsForLanguageHandler = _skillCardFactory.CreateForLanguageHandler(firstPassiveSkill);

        ManaGenerator manaGenerator = new(_manaPerCube, _userSkillFactory.MinManaCost);
        SkillCardDiscoverer skillCardDiscoverer = new(skillCards);

        Instantiate(_manaGeneratorPresenterPrefab, _container).Initialize(manaGenerator);

        UserSkillHandler userSkillHandler = new(skillCardDiscoverer, firstPassiveSkill, _performerPresenter);
        UserSkillPerformer userSkillPerformer = new();

        skillCardDiscoverer.Initialize(userSkillHandler);
        _userSkillScreen.Initialize(userSkillHandler);
        userSkillPerformer.Initialize(userSkillHandler, manaGenerator, _hintAboutUsingSkill, mediator);
        _performerPresenter.Initialize(userSkillPerformer, _userSkillScreen);

        SkillCardDiscoverer = skillCardDiscoverer;

        return userSkillPerformer;
    }

    public List<SkillCardPresenter> GetSkillCardPresenters()
    {
        return _skillCardsForLanguageHandler;
    }

    public List<string> FillActivatedSkills()
    {
        List<string> nameOfActivatedSkills = new ();

        foreach (var skill in _skillsWithThreshold)
        {
            if (skill.Key is FirstLightningStrike)
                nameOfActivatedSkills.Add(skill.Key.GetName());
        }

        return nameOfActivatedSkills;
    }
}
