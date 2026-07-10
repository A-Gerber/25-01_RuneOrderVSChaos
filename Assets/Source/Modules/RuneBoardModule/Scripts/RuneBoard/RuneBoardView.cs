using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class RuneBoardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textLevel;
    [SerializeField] private TextMeshProUGUI _gameScore;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _skillsTooltipButton;
    [SerializeField] private RectTransform _enemyPerformers;

    private IOpenable _menu;

    internal RectTransform EnemyPerformers => _enemyPerformers;

    private void OnEnable()
    {
        _menuButton.onClick.AddListener(() => _menu.Open());
        _skillsTooltipButton.onClick.AddListener(() => _menu.OpenTooltip());
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveListener(() => _menu.Open());
        _skillsTooltipButton.onClick.RemoveListener(() => _menu.OpenTooltip());
    }

    internal void Initialize(IOpenable menu)
    {
        _menu = menu ?? throw new ArgumentNullException("menu is null", nameof(menu));
    }

    internal void UpdateData(int currentLevel, int gameScore)
    {
        if (currentLevel < Constants.StartLevel)
            throw new ArgumentException("currentLevel is not correct", nameof(currentLevel));

        if (gameScore < 0)
            throw new ArgumentException("currentLevel is not correct", nameof(gameScore));

        _textLevel.text = $"{currentLevel}";
        _gameScore.text = $"{gameScore}";
    }
}
