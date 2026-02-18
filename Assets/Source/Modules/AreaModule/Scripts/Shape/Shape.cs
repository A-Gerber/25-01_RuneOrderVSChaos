using System;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    private readonly List<Cube> _cubes = new();
    private readonly MoverTo _moverTo;
    private readonly Transform _transform;
    private readonly float _durationOfReturn;
    private ShapeMover _mover;
    private Vector3 _startPosition;

    private bool _isBackStartPosition = false;
    private bool _isRaised = false;
    private bool _isFrozen = false;

    public Shape(Transform transform, ShapeMover mover, float durationOfReturn)
    {
        if (durationOfReturn <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationOfReturn));

        _transform = transform != null ? transform : throw new InvalidOperationException("transform is null");
        _mover = mover ?? throw new InvalidOperationException("mover is null");
        _durationOfReturn = durationOfReturn;
        _moverTo = new MoverTo(_transform);
    }

    internal event Action<bool> ReleasedOnRestart;

    public bool IsRaised => _isRaised;
    internal bool IsRelease => _cubes.Count == 0;
    internal bool IsFrozen => _isFrozen;
    internal Vector3 StartPosition => _startPosition;

    public bool TryPut()
    {
        if (IsFreeSpace())
        {
            foreach (var cubeModel in _cubes)
            {
                cubeModel.Land();
            }

            _isRaised = false;
            _isFrozen = false;
            ReleasedOnRestart?.Invoke(false);

            return true;
        }
        else
        {
            _isBackStartPosition = true;
            _moverTo.MoveTo(_startPosition, _durationOfReturn);

            return false;
        }
    }

    public void SetStatusRaised()
    {
        _isRaised = true;
        _isBackStartPosition = false;
    }

    internal void SetMover(ShapeMover mover)
    {
        _mover = mover ?? throw new InvalidOperationException("mover is null");
    }

    internal void SetPosition(Vector3 startPosition)
    {
        if (startPosition == null)
            throw new InvalidOperationException("startPosition is null");
      
        _startPosition = startPosition;
    }

    internal void TakeCubes(List<Cube> cubeModels)
    {
        foreach (var cube in cubeModels)
            _cubes.Add(cube);
    }

    internal void FreezeCubes()
    {
        _isFrozen = true;
        
        foreach (var cube in _cubes)
            cube.Freeze();
    }

    internal void RemoveCubes()
    {
        _cubes.Clear();
    }

    internal void ReleaseOnRestart()
    {
        foreach (var cube in _cubes)
        {
            cube.Restart();
        }

        _isFrozen = false;
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
            foreach (var cubeModel in _cubes)
                cubeModel.TrackLanding();
        }
    }

    internal List<LocalPosition> GetLocalPositionCubes()
    {
        List<LocalPosition> localPositions = new();

        foreach (var cubeModel in _cubes)
            localPositions.Add(cubeModel.LocalPosition);

        return localPositions;
    }

    internal void ChangeEffectOnCubes(bool isNormalSize)
    {
        foreach (var cube in _cubes)
            cube.ChangeGlowEffect(isNormalSize);
    }

    private bool IsFreeSpace()
    {
        bool isFreeSpace = true;

        foreach (var cubeModel in _cubes)
        {
            if (cubeModel.TryGetBusyCell())
                isFreeSpace = false;
        }

        return isFreeSpace;
    }
}