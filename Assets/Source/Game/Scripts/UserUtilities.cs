using System;

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
}