using System;
using UnityEngine;

public static class UserUtilities
{
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
}