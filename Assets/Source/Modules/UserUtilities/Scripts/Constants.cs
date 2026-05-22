using System;
using UnityEngine;

public static class Constants
{   
    public static int StartLevel { get; private set; }
    public static int LastLevel { get; private set; }
    public static int ManaCountIncrease { get; private set; }
    public static int AdvertisingReward { get; private set; }
    public static int SkillPointsInterval { get; private set; }
    public static int SkillCountIncrease { get; private set; }
    public static int OriginByX { get; private set; }
    public static int OriginByZ { get; private set; }
    public static int AreaSize { get; private set; }
    public static int EndByX { get; private set; }
    public static int EndByZ { get; private set; }
    public static float HalfDivider { get; private set; } = 2f;
    public static float PercentageMultiplier { get; private set; } = 100f;
    public static float FlightAltitude { get; private set; } = 1.0f;
    public static float CloseDistance { get; private set; } = 0.05f;
    public static float CubeSize { get; private set; }
    public static float CameraHeight { get; private set; }
    public static float CellSize { get; private set; }
    public static float MinBorderArea { get; private set; }
    public static float MaxBorderArea { get; private set; }
    public static Languages Language { get; private set; }
    public static Vector2 MinLimitsForRuneboard { get; private set; }
    public static Vector2 MaxLimitsForRuneboard { get; private set; }

    public static void SetGameParameters(int startLevel, int lastLevel, int manaCountIncrease, int advertisingReward, int skillPointsInterval, int skillCountIncrease)
    {
        if (startLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(startLevel));

        if (lastLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(lastLevel));

        if (manaCountIncrease <= 0)
            throw new ArgumentOutOfRangeException(nameof(manaCountIncrease));

        if (advertisingReward <= 0)
            throw new ArgumentOutOfRangeException(nameof(advertisingReward));

        if (skillPointsInterval <= 0)
            throw new ArgumentOutOfRangeException(nameof(skillPointsInterval));

        if (skillCountIncrease <= 0)
            throw new ArgumentOutOfRangeException(nameof(skillCountIncrease));

        StartLevel = startLevel;
        LastLevel = lastLevel;
        ManaCountIncrease = manaCountIncrease;
        AdvertisingReward = advertisingReward;
        SkillPointsInterval = skillPointsInterval;
        SkillCountIncrease = skillCountIncrease;
    }

    public static void SetAreaParameters(int originByX, int originByZ, Vector2 minLimitsForLeavingArena, Vector2 maxLimitsForLeavingArena, int areaSize)
    {
        if (areaSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(areaSize));

        OriginByX = originByX;
        OriginByZ = originByZ;
        AreaSize = areaSize;
        EndByX = areaSize - 1 + originByX;
        EndByZ = areaSize - 1 + originByZ;
        MinLimitsForRuneboard = minLimitsForLeavingArena;
        MaxLimitsForRuneboard = maxLimitsForLeavingArena;
    }

    public static void SetCubeParameters(float cubeSize)
    {
        if (cubeSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(cubeSize));

        CubeSize = cubeSize;
    }

    public static void SetCameraHeight(float cameraHeight)
    {
        CameraHeight = cameraHeight;
    }

    public static void CalculateAreaBorders(float cellSize)
    {
        if (cellSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(cellSize));

        CellSize = cellSize;

        MinBorderArea = OriginByX - CellSize / HalfDivider;
        MaxBorderArea = EndByX + CellSize / HalfDivider;
    }

    public static void SetLanguage(Languages language)
    {
        Language = language;
    }
}

public enum Languages
{
    Russian,
    English,
    Turkish
}