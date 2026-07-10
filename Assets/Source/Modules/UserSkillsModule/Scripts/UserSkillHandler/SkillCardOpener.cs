using System;
using System.Collections.Generic;

internal class SkillCardOpener
{
    private readonly IReadOnlyList<SkillCard> _deactivatedSkillCards;

    private SkillCard _firstCard;
    private SkillCard _secondCard;
    private SkillCard _thirdCard;
    private SkillCard _passiveCard;

    public SkillCardOpener(IReadOnlyList<SkillCard> deactivatedSkillCards)
    {
        _deactivatedSkillCards = deactivatedSkillCards ?? throw new ArgumentNullException("deactivatedSkillCards is null", nameof(deactivatedSkillCards));
    }

    internal void ClearCards()
    {
        _firstCard = null;
        _secondCard = null;
        _thirdCard = null;
        _passiveCard = null;
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
            case ISettableInFirstButton _:
                SetSkillWithMinThreshold(ref _firstCard, card);
                break;

            case ISettableInSecondButton _:
                SetSkillWithMinThreshold(ref _secondCard, card);
                break;

            case ISettableInThirdButton _:
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
        if (currentCard == null || currentCard.OpeningThreshold > candidate.OpeningThreshold)
            currentCard = candidate;
    }

    private void OpenCards()
    {
        _firstCard?.Open();
        _secondCard?.Open();
        _thirdCard?.Open();
        _passiveCard?.Open();
    }
}