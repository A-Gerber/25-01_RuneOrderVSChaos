using System;
using TMPro;
using UnityEngine;
using YG;

public class LanguageHandler : MonoBehaviour
{
    private TMP_Dropdown _dropdown;
    private ModuleLanguageHandler _moduleLanguageHandler;

    internal void Initialize(ModuleLanguageHandler moduleLanguageHandler, TMP_Dropdown dropdown)
    {
        if (_dropdown != null)
            _dropdown.onValueChanged.RemoveListener((selectedIndex) => SetLanguage((Languages)selectedIndex));

        _moduleLanguageHandler = moduleLanguageHandler != null ? moduleLanguageHandler : throw new ArgumentNullException("moduleLanguageHandler is null", nameof(moduleLanguageHandler));
        _dropdown = dropdown != null ? dropdown : throw new ArgumentNullException("dropdown is null", nameof(dropdown));
        _dropdown.value = (int)Constants.Language;

        if (_dropdown != null)
            _dropdown.onValueChanged.AddListener((selectedIndex) => SetLanguage((Languages)selectedIndex));
    }

    private void SetLanguageInYG2(Languages language)
    {
        if(!enabled)
            return;

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

    private void SetLanguage(Languages language)
    {
        SetLanguageInYG2(language);
        _moduleLanguageHandler.SetLanguage(language);
    }
}
