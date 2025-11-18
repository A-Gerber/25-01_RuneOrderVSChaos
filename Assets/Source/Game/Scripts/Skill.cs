using System;
using System.Collections.Generic;
using UnityEngine;

internal class Skill
{
    private readonly int[,] _configuration;
    private int _offset = -2;

    internal Skill()
    {
        _configuration = new int[,] {
                { 0, 1, 1, 1, 0 },
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
                { 0, 1, 1, 1, 0 }
            };
    }

    internal event Action Used;

    internal List<LocalPosition> GetSkillCoordinates(LocalPosition position, int minBorderArea, int maxBorderArea)
    {       
        List<LocalPosition> coordinates = new List<LocalPosition>();

        for (int i = 0; i < _configuration.GetLength(0); i++)
        {
            for (int j = 0; j < _configuration.GetLength(1); j++)
            {
                if (_configuration[i,j] > 0)
                {
                    int coordinateX = position.PositionX + i + _offset;
                    int coordinateZ = position.PositionZ + j + _offset;

                    if(IsInRangeInt(coordinateX, minBorderArea, maxBorderArea) && IsInRangeInt(coordinateZ, minBorderArea, maxBorderArea))
                        coordinates.Add(new LocalPosition(coordinateX, coordinateZ));
                }
            }
        }

        return coordinates;
    }

    internal void Use()
    {
        Used?.Invoke();
    }

    private bool IsInRangeInt(int value, int min, int max)
    {
        if (min > max)
            throw new ArgumentException("min должен быть <= max");

        return value >= min && value <= max;
    }
}