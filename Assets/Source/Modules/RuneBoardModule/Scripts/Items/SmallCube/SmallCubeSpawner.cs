using System;
using System.Collections.Generic;
using UnityEngine;

internal class SmallCubeSpawner : Spawner<SmallCube>
{
    private readonly List<SmallCube> _currentSmallCubes = new();
    private Vector3 _position;

    internal void Create(Vector3 position)
    {
        _position = position;
        Get();
    }

    internal void Release()
    {
        if (_currentSmallCubes.Count > 0)
        {
            foreach (var cube in _currentSmallCubes)
                cube.Release();

            _currentSmallCubes.Clear();
        }
    }

    protected override void OnRelease(SmallCube smallCube)
    {
        if (smallCube == null)
            throw new InvalidOperationException("bullet is null");

        base.OnRelease(smallCube);

        smallCube.Released -= Release;
    }

    protected override void OnGet(SmallCube smallCube)
    {
        if (smallCube == null)
            throw new InvalidOperationException("bullet is null");

        base.OnGet(smallCube);

        smallCube.transform.position = _position;
        _currentSmallCubes.Add(smallCube);

        smallCube.Released += Release;
    }
}