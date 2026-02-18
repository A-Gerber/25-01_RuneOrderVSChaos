using System;
using UnityEngine;

public static class UserUtilities
{
    private const float MinValueVolumeRegulator = 0.0001f;
    private const float CoefficientVolumeRegulator = 20f;

    public static int StartLevel { get; private set; }
    public static int StartSkillCount { get; private set; }
    public static int SkillIncrease { get; private set; }
    public static int SkillPointsInterval { get; private set; }
    public static int OriginByX { get; private set; }
    public static int OriginByZ { get; private set; }
    public static int AreaSize { get; private set; }
    public static int EndByX { get; private set; }
    public static int EndByZ { get; private set; }
    public static float HalfDivider { get; private set; } = 2f;
    public static float CubeSize { get; private set; }
    public static float CameraHeight { get; private set; }
    public static float CellSize { get; private set; }
    public static float MinBorderArea { get; private set; }
    public static float MaxBorderArea { get; private set; }
    public static bool CanPerformRaycast { get; private set; } = true;


    public static bool IsInRange(float value, float min, float max)
    {
        if (min > max)
            throw new ArgumentException("min должен быть <= max");

        return value >= min && value <= max;
    }

    public static bool IsInRangeInt(int value, int min, int max)
    {
        if (min > max)
            throw new ArgumentException("min должен быть <= max");

        return value >= min && value <= max;
    }

    public static Vector3 GetCursorPosition(float height)
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = height;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    public static bool IsEqualPosition(LocalPosition firstPosition, LocalPosition secondPosition)
    {
        return firstPosition.PositionX == secondPosition.PositionX && firstPosition.PositionZ == secondPosition.PositionZ;
    }

    public static bool IsEqualVector3(Vector3 first, Vector3 second)
    {
        return Mathf.Approximately(first.x, second.x) && Mathf.Approximately(first.y, second.y) && Mathf.Approximately(first.z, second.z);
    }

    public static bool IsLocateInArena(Vector3 targetPosition)
    {
        bool isAbscissaInArea = IsInRange(targetPosition.x, MinBorderArea, MaxBorderArea);
        bool isApplicateInArea = IsInRange(targetPosition.z, MinBorderArea, MaxBorderArea);

        return isAbscissaInArea && isApplicateInArea;
    }

    public static void SetCameraHeight(float cameraHeight)
    {
        CameraHeight = cameraHeight;
    }

    public static void SetAreaParameters(int originByX, int originByZ, int areaSize)
    {
        if (areaSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(areaSize));

        OriginByX = originByX;
        OriginByZ = originByZ;
        AreaSize = areaSize;
        EndByX = areaSize - 1 + originByX;
        EndByZ = areaSize - 1 + originByZ;
    }

    public static void SetGameParameters(int startLevel, int startSkillCount, int skillCountIncrease, int skillPointsInterval)
    {
        if (startLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(startLevel));

        if (startSkillCount < 0)
            throw new ArgumentOutOfRangeException(nameof(startSkillCount));

        if (skillCountIncrease <= 0)
            throw new ArgumentOutOfRangeException(nameof(skillCountIncrease));

        if (skillPointsInterval <= 0)
            throw new ArgumentOutOfRangeException(nameof(skillPointsInterval));

        StartLevel = startLevel;
        StartSkillCount = startSkillCount;
        SkillIncrease = skillCountIncrease;
        SkillPointsInterval = skillPointsInterval;
    }

    public static void SetCubeParameters(float cubeSize)
    {
        if (cubeSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(cubeSize));

        CubeSize = cubeSize;
    }

    public static void CalculateAreaBorders(float cellSize)
    {
        if (cellSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(cellSize));

        CellSize = cellSize;

        MinBorderArea = OriginByX - CellSize / HalfDivider;
        MaxBorderArea = EndByX + CellSize / HalfDivider;
    }

    public static float CalculateVolumeValue(float value)
    {
        if (value < 0 || value > 1f)
            throw new ArgumentOutOfRangeException(nameof(value));

        return Mathf.Log10(Mathf.Max(MinValueVolumeRegulator, value)) * CoefficientVolumeRegulator;
    }

    public static void BanRaycast()
    {
        CanPerformRaycast = false;
    }

    public static void UnbanRaycast()
    {
        CanPerformRaycast = true;
    }
}