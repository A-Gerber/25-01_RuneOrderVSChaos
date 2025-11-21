using System;
using UnityEngine;
using UnityEngine.UI;

internal class EndGameScreen : Window
{
    [SerializeField] private Button _rewardButton;

    internal event Action RestartButtonClicked;
    internal event Action RewardButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        _rewardButton.onClick.AddListener(OnRewardButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _rewardButton.onClick.RemoveListener(OnRewardButtonClick);
    }

    protected override void OnButtonClick()
    {
        RestartButtonClicked?.Invoke();
    }

    private void OnRewardButtonClick()
    {
        RewardButtonClicked?.Invoke();
    }
}
