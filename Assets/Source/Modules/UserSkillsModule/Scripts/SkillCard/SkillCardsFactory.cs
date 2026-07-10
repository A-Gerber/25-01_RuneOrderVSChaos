using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal class SkillCardsFactory : MonoBehaviour
{
    private readonly List<SkillCardPresenter> _skillCardsForLanguageHandler = new();

    [SerializeField] private SkillCardPresenter _skillCardPresenterPrefab;
    [SerializeField] private RectTransform _skillViewContainer;
    [SerializeField] private TextMeshProUGUI _descriptionFirstPassiveSkill;
    [SerializeField] private SkillCardPresenter _firstSkillCard;
    [SerializeField] private int _passiveSkillOfFirstRankThreshold = 1;

    internal List<SkillCard> Create(Dictionary<UserSkill, int> skillsWithThreshold)
    {
        List<SkillCard> skillCards = new();

        foreach (var skill in skillsWithThreshold)
            skillCards.Add(new SkillCard(skill.Key, skill.Value));

        foreach (var card in skillCards)
        {
            SkillCardPresenter skillCard = Instantiate(_skillCardPresenterPrefab, _skillViewContainer);
            skillCard.Initialize(card);
            _skillCardsForLanguageHandler.Add(skillCard);
        }

        return skillCards;
    }

    internal List<SkillCardPresenter> CreateForLanguageHandler(PassiveSkillOfFirstRank firstPassiveSkill)
    {
        SkillCard firstSkillCard = new(firstPassiveSkill, _passiveSkillOfFirstRankThreshold);
        _descriptionFirstPassiveSkill.text = firstPassiveSkill.SkillDescription;

        _firstSkillCard.Initialize(firstSkillCard);
        _skillCardsForLanguageHandler.Add(_firstSkillCard);

        return _skillCardsForLanguageHandler;
    }
}