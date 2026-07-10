using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncreasedDamageScreen : Window
{
    [SerializeField] private Button _rewardButton;
    [SerializeField] private TextMeshProUGUI _damageIncreaseText;

    public event Action Opened;
    internal event Action RewardButtonClicked;

    public bool IsOpen { get; private set; } = false;

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

    public override void Close()
    {
        base.Close();
        IsOpen = false;
    }

    public override void Open()
    {
        base.Open();
        IsOpen = true;
        Opened?.Invoke();
    }

    protected override void OnExitButtonClick()
    {
        Close();
    }

    private void OnRewardButtonClick()
    {
        RewardButtonClicked?.Invoke();
        Close();
    }
}