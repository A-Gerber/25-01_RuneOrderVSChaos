using System;

internal class GameSavedData
{
    internal GameSavedData(int level, int manaCount, int gameScore)
    {
        if (level < 0)
            throw new ArgumentOutOfRangeException(nameof(level));

        if (manaCount < 0)
            throw new ArgumentOutOfRangeException(nameof(manaCount));

        if (GameScore < 0)
            throw new ArgumentOutOfRangeException(nameof(GameScore));

        Level = level;
        ManaCount = manaCount;
        GameScore = gameScore;
    }

    internal int Level { get; private set; }
    internal int ManaCount { get; private set; }
    internal int GameScore { get; private set; }
}