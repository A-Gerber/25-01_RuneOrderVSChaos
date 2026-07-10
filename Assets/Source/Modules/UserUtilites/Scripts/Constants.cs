using System;

public static class Constants
{   
    public static int OriginByX { get; private set; } = 0;
    public static int OriginByZ { get; private set; } = 0;
    public static int AreaSize { get; private set; } = 8;
    public static int ShapeCountForArea { get; private set; } = 3;
    public static int SkillPointsInterval { get; private set; } = 5;
    public static int SkillCountIncrease { get; private set; } = 1;
    public static int ShapeCountForCreate { get; private set; } = 3;
    public static float HalfDivider { get; private set; } = 2f;
    public static float PercentageMultiplier { get; private set; } = 100f;
    public static float FlightAltitude { get; private set; } = 1.0f;
    public static float CloseDistance { get; private set; } = 0.05f;
    public static int StartLevel { get; private set; }
    public static int LastLevel { get; private set; }
    public static int ManaCountIncrease { get; private set; }
    public static int AdvertisingReward { get; private set; }
    public static float UnitCoefficient { get; private set; } = 1f;
    public static float CubeSize { get; private set; }
    public static float CameraHeight { get; private set; }
    public static float CellSize { get; private set; }
    public static float MinBorderArea { get; private set; }
    public static float MaxBorderArea { get; private set; }
    public static Languages Language { get; private set; }
    public static int EndByX => AreaSize - 1 + OriginByX;
    public static int EndByZ => AreaSize - 1 + OriginByZ;

    public static void SetGameParameters(int startLevel, int lastLevel, int manaCountIncrease, int advertisingReward)
    {
        if (startLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(startLevel));

        if (lastLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(lastLevel));

        if (manaCountIncrease <= 0)
            throw new ArgumentOutOfRangeException(nameof(manaCountIncrease));

        if (advertisingReward <= 0)
            throw new ArgumentOutOfRangeException(nameof(advertisingReward));


        StartLevel = startLevel;
        LastLevel = lastLevel;
        ManaCountIncrease = manaCountIncrease;
        AdvertisingReward = advertisingReward;
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

    public static void CalculateAreaParameters(float cellSize, float cameraHeight)
    {
        if (cellSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(cellSize));

        CellSize = cellSize;
        CameraHeight = cameraHeight;

        MinBorderArea = OriginByX - CellSize / HalfDivider;
        MaxBorderArea = EndByX + CellSize / HalfDivider;
    }

    public static void SetLanguage(Languages language)
    {
        Language = language;
    }
}