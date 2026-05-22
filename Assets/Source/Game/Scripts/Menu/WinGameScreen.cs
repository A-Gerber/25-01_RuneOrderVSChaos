using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal class WinGameScreen : Window
{
    [SerializeField] private CanvasGroup _witchGroup;
    [SerializeField] private RectTransform _scrollView;
    [SerializeField] private RectTransform _iconSkillContainer;
    [SerializeField] private SkillIcon _skillIconPrefab;
    [SerializeField] private TextMeshProUGUI _textGameScoreIncrease;
    [SerializeField] private TextMeshProUGUI _textSkillCountIncrease;
    [SerializeField] private TextMeshProUGUI _textSkillScoreIncrease;
    [SerializeField] private TextMeshProUGUI _levelRemainder;
    [SerializeField] private TextMeshProUGUI _remainderText;

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
        _remainderText.gameObject.SetActive(false);

        foreach (var sprite in sprites)
        {
            SkillIcon skillIcon = Instantiate(_skillIconPrefab, _iconSkillContainer);
            skillIcon.SetIcon(sprite);
            _skillIcons.Add(skillIcon);
        }
    }

    internal void HideSkills(int threshold, int currentLevel)
    {
        if (threshold - currentLevel > 0)
        {
            _remainderText.gameObject.SetActive(true);
            _levelRemainder.text = $"<size=50>{threshold - currentLevel}<size=30> lv";
        }
        else
        {
            _remainderText.gameObject.SetActive(false);
        }

        if (_skillIcons.Count == 0)
            return;

        ClearSkillIcons();
        _scrollView.gameObject.SetActive(false);
    }

    internal void UpdateIncreases(int gameScoreIncrease, int currentLevel)
    {
        if (gameScoreIncrease < 0)
            throw new ArgumentOutOfRangeException(nameof(gameScoreIncrease));

        if (currentLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(currentLevel));

        _textGameScoreIncrease.text = $"+{gameScoreIncrease}";
        _textSkillCountIncrease.text = $"+{Constants.ManaCountIncrease}";

        if ((currentLevel) % Constants.SkillPointsInterval == 0)
            _textSkillScoreIncrease.text = $"+{Constants.SkillCountIncrease}";
        else
            _textSkillScoreIncrease.text = $"+0";
    }

    internal void ShowWitch()
    {
        _witchGroup.alpha = 1f;
    }

    internal void HideWitch()
    {
        _witchGroup.alpha = 0f;
    }

    private void ClearSkillIcons()
    {
        foreach (var icon in _skillIcons)
            Destroy(icon.gameObject);

        _skillIcons.Clear();
    }
}
