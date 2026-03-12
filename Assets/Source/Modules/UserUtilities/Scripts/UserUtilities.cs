using System;
using UnityEngine;

public static class UserUtilities
{
    private const float MinValueVolumeRegulator = 0.0001f;
    private const float CoefficientVolumeRegulator = 20f;

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
        bool isAbscissaInArea = IsInRange(targetPosition.x, Constants.MinBorderArea, Constants.MaxBorderArea);
        bool isApplicateInArea = IsInRange(targetPosition.z, Constants.MinBorderArea, Constants.MaxBorderArea);

        return isAbscissaInArea && isApplicateInArea;
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

    public static Vector3 GetRandomScreenPosition()
    {
        float x = UnityEngine.Random.Range(Constants.MinBorderArea, Constants.MaxBorderArea);
        float z = UnityEngine.Random.Range(Constants.MinBorderArea, Constants.MaxBorderArea);

        Vector3 randomPosition = new(x, 0f, z);
        return Camera.main.WorldToScreenPoint(randomPosition);
    }
}
