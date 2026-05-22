using System;
using UnityEngine;
using YG;

internal class AdvertisementViewer : MonoBehaviour
{
    private const string RewardID = "AddMana";
    private IAdvertisementViewer _menuView;
    private IRewardable _game;

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    internal void Initialize(IAdvertisementViewer menuView, IRewardable game)
    {
        Unsubscribe();

        _menuView = menuView ?? throw new InvalidOperationException("menuView is null");
        _game = game ?? throw new InvalidOperationException("game is null");

        Subscribe();
    }

    private void OnInterstitialADVShow()
    {
        YG2.InterstitialAdvShow();
    }

    private void OnRewardedADVShow()
    {
        YG2.RewardedAdvShow(RewardID, Reward);
    }

    private void Reward()
    {
        _game.RewardForADV();
    }

    private void Subscribe()
    {
        if (_menuView != null)
        {
            _menuView.ClickedRewardButton += OnRewardedADVShow;
            _menuView.ClickedNextButton += OnInterstitialADVShow;
        }
    }

    private void Unsubscribe()
    {
        if (_menuView != null)
        {
            _menuView.ClickedRewardButton -= OnRewardedADVShow;
            _menuView.ClickedNextButton -= OnInterstitialADVShow;
        }
    }
}