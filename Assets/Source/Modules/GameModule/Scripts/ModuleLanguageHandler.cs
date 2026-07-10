using System;
using System.Collections.Generic;
using UnityEngine;

public class ModuleLanguageHandler : MonoBehaviour
{
    private  List<SkillCardPresenter> _skillCards = new();
    private IChangeableLanguage _enemyPresenter;

    public void SetLanguage(Languages language)
    {
        _enemyPresenter.ChangeSkillDescription(language);

        foreach (var skillCard in _skillCards)
            skillCard.ChangeSkillDescription(language);

        Constants.SetLanguage(language);
    }

    internal void Initialize(IChangeableLanguage enemyPresenter, List<SkillCardPresenter> skillCards)
    {
        if (skillCards == null)
            throw new ArgumentNullException("skillCards is null", nameof(skillCards));

        if (skillCards.Count == 0)
            throw new InvalidOperationException("skillCards is empty");

        _skillCards = skillCards;
        _enemyPresenter = enemyPresenter ?? throw new ArgumentNullException("enemyPresenter is null", nameof(enemyPresenter));
    }
}