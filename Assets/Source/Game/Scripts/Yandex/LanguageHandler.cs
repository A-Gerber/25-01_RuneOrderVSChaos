using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

internal class LanguageHandler : MonoBehaviour
{
    private readonly List<SkillCardView> _skillCards = new();

    [SerializeField] private List<TMP_Dropdown> _dropdowns;
    [SerializeField] private EnemyPresenter _enemyPresenter;

    private int _currentLanguage = -1;

    private void OnEnable()
    {
        foreach (var dropdown in _dropdowns)
            dropdown.onValueChanged.AddListener(OnHandleDropdownChange);
    }

    private void OnDisable()
    {
        foreach (var dropdown in _dropdowns)
            dropdown.onValueChanged.RemoveListener(OnHandleDropdownChange);
    }

    private void Start()
    {
        EnableDropdown((int)Constants.Language);
    }

    internal void Initialize(List<SkillCardView> skillCards)
    {
        if (skillCards == null)
            throw new InvalidOperationException("skillCards is null");

        if (skillCards.Count == 0)
            throw new InvalidOperationException("skillCards is empty");

        _skillCards.AddRange(skillCards);
    }

    internal void SetLanguage(Languages language)
    {
        int languageIndex = (int)language;

        if (_currentLanguage != languageIndex)
        {
            EnableDropdown(languageIndex);

            for (int i = 0; i < _dropdowns.Count; i++)
                _dropdowns[i].value = languageIndex;

            SetLanguageInYG2(language);
            _enemyPresenter.ChangeSkillDescription(language);

            foreach (var skillCard in _skillCards)
                skillCard.ChangeSkillDescription(language);

            Constants.SetLanguage(language);

            _currentLanguage = languageIndex;
        }
    }

    private void EnableDropdown(int languageIndex)
    {
        for (int i = 0; i < _dropdowns.Count; i++)
            _dropdowns[i].gameObject.SetActive(i == languageIndex);
    }

    private void SetLanguageInYG2(Languages language)
    {
        switch (language)
        {
            case Languages.Russian:
                YG2.SwitchLanguage("ru");
                break;

            case Languages.Turkish:
                YG2.SwitchLanguage("tr");
                break;

            default:
                YG2.SwitchLanguage("en");
                break;
        }
    }

    private void OnHandleDropdownChange(int selectedIndex)
    {
        SetLanguage((Languages)selectedIndex);
    }
}