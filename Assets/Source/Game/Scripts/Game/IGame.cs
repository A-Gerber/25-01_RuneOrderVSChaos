using System;

internal interface IGame : IWinable
{
    event Action GameOvered;

    bool IsPlaying { get; }
    int CurrentLevel { get; }

    void StartNewGame();

    void Restart();

    void GoToNextLevel();

    void OnRewardSkillPoints(int numberOfSkillPoints);
}

internal interface IWinable
{
    event Action<int> GameWined;
}