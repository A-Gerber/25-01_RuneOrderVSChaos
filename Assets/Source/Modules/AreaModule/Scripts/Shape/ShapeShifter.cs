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
    private const float Shift = 1f;

    internal override float CalculateOffset(List<Cube> cubes)
    {
        List<int> values = new();

        foreach (var cube in cubes)
            values.Add(cube.LocalPosition.PositionZ);

        return Shift + (values.Max() - values.Min()) / Constants.HalfDivider;
    }
}