using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal class WinGameScreen : Window
{
    [SerializeField] private RectTransform _scrollView;
    [SerializeField] private RectTransform _iconSkillContainer;
    [SerializeField] private SkillIcon _skillIconPrefab;
    [SerializeField] private TextMeshProUGUI _textGameScoreIncrease;
    [SerializeField] private TextMeshProUGUI _textSkillCountIncrease;
    [SerializeField] private TextMeshProUGUI _textSkillScoreIncrease;

    private readonly List<SkillIcon> _skillIcons = new();

    internal event Action NextLevelButtonClicked;

    protected override void OnButtonClick()
    {
        NextLevelButtonClicked?.Invoke();
    }

    internal void ShowOpenSkills(List<Sprite> sprites)
    {
        if (_skillIcons.Count != 0)
            ClearSkillIcons();

        _scrollView.gameObject.SetActive(true);

        foreach (var sprite in sprites)
        {
            SkillIcon skillIcon = Instantiate(_skillIconPrefab, _iconSkillContainer);
            skillIcon.SetIcon(sprite);
            _skillIcons.Add(skillIcon);
        }
    }

    internal void Hide()
    {
        if (_skillIcons.Count == 0)
            return;

        ClearSkillIcons();
        _scrollView.gameObject.SetActive(false);
    }

    internal void UpdateIncreases(int gameScoreIncrease, int currentLevel)
    {
        if (gameScoreIncrease <= 0)
            throw new ArgumentOutOfRangeException(nameof(gameScoreIncrease));

        if (currentLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(currentLevel));

        _textGameScoreIncrease.text = $"+{gameScoreIncrease}";
        _textSkillCountIncrease.text = $"+{UserUtilities.SkillIncrease}";

        if ((currentLevel + 1) % UserUtilities.SkillPointsInterval == 0)
            _textSkillScoreIncrease.text = $"+{UserUtilities.SkillIncrease}";
        else
            _textSkillScoreIncrease.text = $"+0";
    }

    private void ClearSkillIcons()
    {
        foreach (var icon in _skillIcons)
            Destroy(icon.gameObject);

        _skillIcons.Clear();
    }
}
