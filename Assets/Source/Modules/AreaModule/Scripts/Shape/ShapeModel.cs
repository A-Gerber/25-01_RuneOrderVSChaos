using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapeModel : ILiftable
{
    private readonly List<CubeModel> _cubeModels = new();
    private readonly MoverTo _moverTo;
    private readonly ShapeMover _mover;
    private readonly Transform _transform;
    private readonly float _durationOfReturn;
    private Vector3 _startPosition;

    private bool _isBackStartPosition = false;
    private bool _isRaised = false;

    public ShapeModel(ShapeMover mover, Transform transform, float durationOfReturn)
    {
        _mover = mover ?? throw new InvalidOperationException("mover is null");
        _transform = transform != null ? transform : throw new InvalidOperationException("transform is null");

        if (durationOfReturn <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationOfReturn));

        _durationOfReturn = durationOfReturn;
        _moverTo = new MoverTo(_transform);
    }

    internal event Action<bool> ReleasedOnRestart;

    public bool IsRaised => _isRaised;
    internal bool IsRelease => _cubeModels.Count == 0;

    public void Put()
    {
        if (IsFreeSpace())
        {
            foreach (var cubeModel in _cubeModels)
            {
                cubeModel.Land();
            }

            _isRaised = false;
            ReleasedOnRestart?.Invoke(false);
        }
        else
        {
            _isBackStartPosition = true;
            _moverTo.MoveTo(_startPosition, _durationOfReturn);
        }
    }

    public void SetStatusRaised()
    {
        _isRaised = true;
        _isBackStartPosition = false;
    }

    internal void SetPosition(Vector3 startPosition)
    {
        if (startPosition == null)
            throw new InvalidOperationException("startPosition is null");

        _startPosition = startPosition;
    }

    internal void TakeCubes(List<CubeModel> cubeModels)
    {
        foreach (var cube in cubeModels)
        {
            _cubeModels.Add(cube);
            cube.SetLiftableShape(this);
        }
    }

    internal void RemoveCubes()
    {
        _cubeModels.Clear();
    }

    internal void ReleaseOnRestart()
    {
        foreach (var cube in _cubeModels)
        {
            cube.Release();
        }

        ReleasedOnRestart?.Invoke(true);
    }

    internal void SetStatusOnStartPoint()
    {
        _isRaised = false;
    }

    internal void Raise()
    {
        _mover.Move(_transform);

        if (_isBackStartPosition == false)
        {
            foreach (var cubeModel in _cubeModels)
                cubeModel.TrackLanding();
        }
    }

    internal List<LocalPosition> GetLocalPositionCubes()
    {
        List<LocalPosition> localPositions = new();

        foreach (var cubeModel in _cubeModels)
            localPositions.Add(cubeModel.LocalPosition);

        return localPositions;
    }

    private bool IsFreeSpace()
    {
        bool isFreeSpace = true;

        foreach (var cubeModel in _cubeModels)
        {
            if (cubeModel.TryGetBusyCell())
                isFreeSpace = false;
        }

        return isFreeSpace;
    }
}