using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillCardDiscoverer : IShowableNextSkills
{
    private readonly IReadOnlyList<SkillCard> _skillCards;
    private readonly List<SkillCard> _deactivatedSkillCards = new();
    private readonly SkillCardOpener _opener;

    public SkillCardDiscoverer(List<SkillCard> skillCards)
    {
        _skillCards = skillCards ?? throw new ArgumentNullException("skillCards is null", nameof(skillCards));

        foreach (var card in _skillCards)
            _deactivatedSkillCards.Add(card);

        _opener = new SkillCardOpener(_deactivatedSkillCards);
    }

    public bool TryGetSkillSprites(out List<Sprite> sprites, int currentLevel)
    {
        sprites = new List<Sprite>();

        foreach (var card in _skillCards)
        {
            if (card.OpeningThreshold == currentLevel)
                sprites.Add(card.Skill.IconOnButton);
        }

        return sprites.Count > 0;
    }

    public int GetNextThreshold(int currentLevel)
    {
        List<int> thresholds = new();

        foreach (var card in _skillCards)
        {
            if (card.OpeningThreshold > currentLevel)
                thresholds.Add(card.OpeningThreshold);
        }

        if (thresholds.Count > 0)
            return thresholds.Min();

        return 0;
    }

    internal void Initialize(IAddableSkill userSkillHandler)
    {
        foreach (var card in _skillCards)
            card.Initialize(userSkillHandler);
    }

    internal void ActivateSkillCards(List<string> activatedSkills)
    {
        foreach (var skill in activatedSkills)
        {
            foreach (var card in _skillCards)
            {
                if (card.Skill.GetName() == skill)
                    card.ActivateOnLoad();
            }
        }
    }

    internal List<string> GetActivatedSkills()
    {
        List<string> activatedSkills = new();

        foreach (var card in _skillCards)
        {
            if (card.IsActivated)
                activatedSkills.Add(card.Skill.GetName());
        }

        return activatedSkills;
    }

    internal void RemoveFromClosedList(SkillCard skillCard)
    {
        _deactivatedSkillCards.Remove(skillCard);
    }

    internal void SetInteracteble(bool value)
    {
        foreach (var card in _deactivatedSkillCards)
        {
            if (card.IsOpen)
                card.SetInteracteble(value);
        }
    }

    internal void Reset()
    {
        _deactivatedSkillCards.Clear();
        _opener.ClearCards();

        foreach (var card in _skillCards)
        {
            card.Close();
            _deactivatedSkillCards.Add(card);
        }
    }

    internal void OpenSkillCards(int level)
    {
        _opener.OpenSkillCards(level);
    }
}
