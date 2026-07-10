using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class EndGameScreen : Window
{
    [SerializeField] private Button _rewardButton;
    [SerializeField] private TextMeshProUGUI _manaIncrease;
    [SerializeField] private RectTransform _witchImage;

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

    public override void Close()
    {
        base.Close();
        _witchImage.gameObject.SetActive(false);
    }

    public void ChangeManaIncrease(int manaIncrease)
    {
        _manaIncrease.text = $"+{manaIncrease}";
    }

    internal void ShowWitch()
    {
        _witchImage.gameObject.SetActive(true);
    }

    protected override void OnExitButtonClick()
    {
        RestartButtonClicked?.Invoke();
    }

    private void OnRewardButtonClick()
    {
        RewardButtonClicked?.Invoke();
    }
}