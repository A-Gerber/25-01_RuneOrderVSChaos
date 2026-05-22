using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeShifter
{
    virtual internal float CalculateOffset(List<Cube> cubes)
    {
        return 0f;
    }
}

public class MobileShapeShifter : ShapeShifter
{
    private readonly float _shift;

    public MobileShapeShifter(float shift)
    {
        if (shift < 0)
            throw new ArgumentOutOfRangeException(nameof(shift));

        _shift = shift;
    }

    internal override float CalculateOffset(List<Cube> cubes)
    {
        List<int> values = new();

        foreach (var cube in cubes)
            values.Add(cube.LocalPosition.PositionZ);

        return _shift + Mathf.Ceil((values.Max() - values.Min()) / Constants.HalfDivider);
    }
}