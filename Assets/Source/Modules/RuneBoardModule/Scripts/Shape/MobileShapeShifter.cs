using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class MobileShapeShifter : ShapeShifter
{
    private readonly float _shift;

    internal MobileShapeShifter(float shift)
    {
        if (shift < 0)
            throw new ArgumentOutOfRangeException(nameof(shift));

        _shift = shift;
    }

    internal override float CalculateOffset(List<LocalPosition> cubePositions)
    {
        List<int> values = new();

        foreach (var position in cubePositions)
            values.Add(position.Z);

        return _shift + Mathf.Ceil((values.Max() - values.Min()) / Constants.HalfDivider);
    }
}