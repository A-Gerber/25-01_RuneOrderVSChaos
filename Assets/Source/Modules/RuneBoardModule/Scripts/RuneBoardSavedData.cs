using System;

public class RuneBoardSavedData
{
    public RuneBoardSavedData(int level, int gameScore)
    {
        if (level < 0)
            throw new ArgumentOutOfRangeException(nameof(level));

        if (GameScore < 0)
            throw new ArgumentOutOfRangeException(nameof(GameScore));

        Level = level;
        GameScore = gameScore;
    }

    public int Level { get; private set; }
    public int GameScore { get; private set; }
}