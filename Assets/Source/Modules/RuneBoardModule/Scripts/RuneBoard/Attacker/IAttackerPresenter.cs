using System;

public interface IAttackerPresenter
{
    public event Action RewardButtonClicked;

    public void RewardDamage(int value);
}