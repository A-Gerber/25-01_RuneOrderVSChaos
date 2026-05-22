using System;

internal interface IGame : IWinable
{
    event Action<int> GameOvered;

    bool IsPlaying { get; }
    int CurrentLevel { get; }

    void StartGame(GameSavedData data);

    void Restart();

    void GoToNextLevel();
}

internal interface IWinable
{
    event Action<GameSavedData> GameWined;
}

internal interface IRewardable
{
    void RewardForADV();
}