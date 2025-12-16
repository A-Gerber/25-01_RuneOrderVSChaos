using System;
using System.Collections.Generic;
using UnityEngine;

internal class SkillCardDiscoverer
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

    internal void InitializeSkillCards(IAddableSkill userSkillHandler)
    {
        foreach (var card in _skillCards)
            card.Initialize(userSkillHandler);
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