using System;

internal interface IGame
{
    event Action GameOvered;
    event Action GameWined;

    bool IsPlaying { get; }

    void NewGame();

    void Restart();

    void GoToNextLevel();

    void OnRewardSkillPoints(int numberOfSkillPoints);
}