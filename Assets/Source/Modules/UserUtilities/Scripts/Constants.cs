using System;
using UnityEngine;

public static class Constants
{
    public static int StartLevel { get; private set; }
    public static int LastLevel { get; private set; }
    public static int StartSkillCount { get; private set; }
    public static int SkillIncrease { get; private set; }
    public static int RewardForAdvertising { get; private set; }
    public static int SkillPointsInterval { get; private set; }
    public static int OriginByX { get; private set; }
    public static int OriginByZ { get; private set; }
    public static int AreaSize { get; private set; }
    public static int EndByX { get; private set; }
    public static int EndByZ { get; private set; }
    public static float HalfDivider { get; private set; } = 2f;
    public static float PercentageMultiplier { get; private set; } = 100f;
    public static float CubeSize { get; private set; }
    public static float CameraHeight { get; private set; }
    public static float FlightAltitude { get; private set; }
    public static float CellSize { get; private set; }
    public static float MinBorderArea { get; private set; }
    public static float MaxBorderArea { get; private set; }
    public static Vector2 MinLimitsForLeavingArena { get; private set; }
    public static Vector2 MaxLimitsForLeavingArena { get; private set; }

    public static void SetGameParameters(int startLevel, int lastLevel, int startSkillCount, int skillCountIncrease, int skillPointsInterval)
    {
        if (startLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(startLevel));

        if (lastLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(lastLevel));

        if (startSkillCount < 0)
            throw new ArgumentOutOfRangeException(nameof(startSkillCount));

        if (skillCountIncrease <= 0)
            throw new ArgumentOutOfRangeException(nameof(skillCountIncrease));

        if (skillPointsInterval <= 0)
            throw new ArgumentOutOfRangeException(nameof(skillPointsInterval));

        StartLevel = startLevel;
        LastLevel = lastLevel;
        StartSkillCount = startSkillCount;
        SkillIncrease = skillCountIncrease;
        SkillPointsInterval = skillPointsInterval;
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
        MinLimitsForLeavingArena = minLimitsForLeavingArena;
        MaxLimitsForLeavingArena = maxLimitsForLeavingArena;
    }

    public static void SetCubeParameters(float cubeSize)
    {
        if (cubeSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(cubeSize));

        CubeSize = cubeSize;
    }

    public static void SetCameraHeight(float cameraHeight, float flightAltitude)
    {
        CameraHeight = cameraHeight;
        FlightAltitude = flightAltitude;
    }

    public static void CalculateAreaBorders(float cellSize)
    {
        if (cellSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(cellSize));

        CellSize = cellSize;

        MinBorderArea = OriginByX - CellSize / HalfDivider;
        MaxBorderArea = EndByX + CellSize / HalfDivider;
    }

    public static void SetRewardForAdvertising(int rewardForAdvertising)
    {
        if (rewardForAdvertising <= 0)
            throw new ArgumentOutOfRangeException(nameof(rewardForAdvertising));

        RewardForAdvertising = rewardForAdvertising;
    }
}
