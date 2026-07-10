using System;
using UnityEngine;
using YG;

internal class AdvertisementViewer : MonoBehaviour
{
    private const string ManaRewardID = "AddMana";
    private const string DamageRewardID = "AddDamage";
    private const int DamageRewardValue = 1;

    private Game _game;
    private FinalGameHandler _finalGameHandler;
    private IAttackerPresenter _attackerPresenter;

    internal void Initialize(Game game, FinalGameHandler finalGameHandler, IAttackerPresenter attackerPresenter)
    {
        if (_finalGameHandler != null)
        {
            _finalGameHandler.RewardButtonClicked -= () => { if (enabled) YG2.RewardedAdvShow(ManaRewardID, RewardMana); };
            _finalGameHandler.NextLevelButtonClicked -= () => { if (enabled) YG2.InterstitialAdvShow(); };
        }

        if (_attackerPresenter != null)
            _attackerPresenter.RewardButtonClicked -= OnRewardDamage;


        _game = game ??  throw new ArgumentNullException("game is null", nameof(game));
        _finalGameHandler = finalGameHandler != null ? finalGameHandler : throw new ArgumentNullException("finalGameHandler is null", nameof(finalGameHandler));
        _attackerPresenter = attackerPresenter ?? throw new ArgumentNullException("attackerPresenter is null", nameof(attackerPresenter));

        if (_finalGameHandler != null)
        {
            _finalGameHandler.RewardButtonClicked += () => { if (enabled) YG2.RewardedAdvShow(ManaRewardID, RewardMana); };
            _finalGameHandler.NextLevelButtonClicked += () => { if (enabled) YG2.InterstitialAdvShow(); };
        }

        if (_attackerPresenter != null)
            _attackerPresenter.RewardButtonClicked += OnRewardDamage;
    }

    private void RewardMana()
    {
        _game.RewardForADV();
    }

    private void OnRewardDamage()
    {
        if (enabled) 
            YG2.RewardedAdvShow(DamageRewardID, RewardDamage);
    }

    private void RewardDamage()
    {
        _attackerPresenter.RewardDamage(DamageRewardValue);
    }
}
