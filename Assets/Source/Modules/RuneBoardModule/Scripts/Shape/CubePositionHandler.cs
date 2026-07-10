using System.Collections.Generic;
using UnityEngine;

internal class CubePositionHandler
{
    private readonly List<LocalPosition> _cubePositions = new();

    internal List<LocalPosition> GetCubePositions(List<Cube> cubes)
    {
        _cubePositions.Clear();

        foreach (var cube in cubes)
        {
            if (UserUtilities.IsLocateInArena(cube.Position))
                _cubePositions.Add(new LocalPosition(Mathf.RoundToInt(cube.Position.x), Mathf.RoundToInt(cube.Position.z)));
        }

        return _cubePositions;
    }
}