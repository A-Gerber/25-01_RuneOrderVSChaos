using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillCardDiscoverer : ISkillCardDiscoverer
{
    private readonly IReadOnlyList<SkillCard> _skillCards;
    private readonly List<SkillCard> _deactivatedSkillCards = new();

    private SkillCard _firstCard;
    private SkillCard _secondCard;
    private SkillCard _thirdCard;
    private SkillCard _passiveCard;

    public SkillCardDiscoverer(List<SkillCard> skillCards)
    {
        _skillCards = skillCards ?? throw new InvalidOperationException("skillCards is null");

        foreach (var card in _skillCards)
            _deactivatedSkillCards.Add(card);
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

        return thresholds.Min();
    }

    internal void InitializeSkillCards(IAddableSkill userSkillHandler)
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
        ClearCards();

        foreach (var card in _skillCards)
        {
            card.Close();
            _deactivatedSkillCards.Add(card);
        }
    }

    internal void OpenSkillCards(int level)
    {
        bool isOpenSkills = false;

        foreach (var card in _deactivatedSkillCards)
        {
            if (card.OpeningThreshold <= level)
            {
                SortSkillCard(card);
                isOpenSkills = true;
            }
        }

        if (isOpenSkills)
        {
            OpenCards();
            ClearCards();
        }
    }

    private void SortSkillCard(SkillCard card)
    {
        switch (card.Skill)
        {
            case ISetableInFirstButton _:
                SetSkillWithMinThreshold(ref _firstCard, card);
                break;

            case ISetableInSecondButton _:
                SetSkillWithMinThreshold(ref _secondCard, card);
                break;

            case ISetableInThirdButton _:
                SetSkillWithMinThreshold(ref _thirdCard, card);
                break;

            case IPassiveSkill _:
                SetSkillWithMinThreshold(ref _passiveCard, card);
                break;

            default:
                break;
        }
    }

    private void SetSkillWithMinThreshold(ref SkillCard currentCard, SkillCard candidate)
    {
        if (currentCard != null)
        {
            if (currentCard.OpeningThreshold > candidate.OpeningThreshold)
                currentCard = candidate;
        }
        else
        {
            currentCard = candidate;
        }
    }

    private void OpenCards()
    {
        _firstCard?.Open();
        _secondCard?.Open();
        _thirdCard?.Open();
        _passiveCard?.Open();
    }

    private void ClearCards()
    {
        _firstCard = null;
        _secondCard = null;
        _thirdCard = null;
        _passiveCard = null;
    }
}