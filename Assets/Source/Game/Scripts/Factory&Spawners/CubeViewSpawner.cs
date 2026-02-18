using System;
using System.Collections.Generic;
using UnityEngine;

internal class CubeViewSpawner : Spawner<CubeView>, ICreateableCubesForArea
{
    private readonly List<CubeView> _currentCubeViews = new();
    private List<LocalPosition> _coordinates;
    List<CellModel> _cells = new();
    private int _index = 0;

    private bool _isSendForShape = true;

    internal event Action<List<CubeView>> CreatedCubeView;

    public void CreateCubesForArea(List<LocalPosition> coordinates, List<CellModel> cells)
    {
        if (coordinates == null)
            throw new InvalidOperationException("coordinate is null");

        if (cells == null)
            throw new InvalidOperationException("cells is null");

        _coordinates = coordinates;
        _cells = cells;
        _isSendForShape = false;

        for (int i = 0; i < coordinates.Count; i++)
        {
            Get();
        }
    }

    internal void CreateCubesForShape(List<LocalPosition> coordinates)
    {
        if (coordinates == null)
            throw new InvalidOperationException("coordinate is null");

        _coordinates = coordinates;
        _isSendForShape = true;

        for (int i = 0; i < coordinates.Count; i++)
        {
            Get();
        }
    }

    protected override CubeView Create()
    {
        CubeView @object = Instantiate(Prefab);
        @object.Initialize(new Cube(@object.transform, @object.Rigidbody, @object.DurationLanding, @object.RaycastDistance));

        return @object;
    }

    protected override void OnRelease(CubeView cube)
    {
        if (cube == null)
            throw new InvalidOperationException("cube is null");

        cube.Reset();
        base.OnRelease(cube);

        cube.Released -= Release;
    }

    protected override void OnGet(CubeView cube)
    {
        if (cube == null)
            throw new InvalidOperationException("cube is null");

        base.OnGet(cube);

        cube.SetLocalPosition(_coordinates[_index]);
        _currentCubeViews.Add(cube);

        SendCubeViews();

        cube.Released += Release;
    }

    private void SendCubeViews()
    {
        if (_index == _coordinates.Count - 1)
        {
            if (_isSendForShape)
                CreatedCubeView?.Invoke(_currentCubeViews);
            else
                FillCells();

            _index = 0;
            _currentCubeViews.Clear();
        }
        else
        {
            _index++;
        }
    }

    private void FillCells()
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            _currentCubeViews[i].transform.position = new Vector3(_coordinates[i].PositionX,0, _coordinates[i].PositionZ);
            _cells[i].Take(_currentCubeViews[i].GetCubeModel());
        }

        _cells.Clear();
    }
}