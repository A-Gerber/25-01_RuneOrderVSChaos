using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class Shape : IChangableCubeEffect
{
    private readonly List<Cube> _cubes = new();
    private readonly List<LocalPosition> _cubePositions = new();
    private readonly ShapeLander _shapeLander = new();
    private readonly CubePositionHandler _positionHandler = new();
    private readonly Transform _transform;
    private readonly ShapeShifter _shapeShifter;
    private readonly MoverBehindCursor _mover;
    private readonly IDisplayChangeable _runeDisplayer;

    private readonly TransparencyState _transparencyState = new(true);
    private readonly SmallState _defaultSizeState = new(false);
    private readonly SmallState _smallSizeState = new(true);

    private float _verticalShift;

    internal Shape(Transform transform, ShapeShifter shapeShifter, MoverBehindCursor mover, IDisplayChangeable runeDisplayer)
    {
        _transform = transform != null ? transform : throw new InvalidOperationException("transform is null");
        _shapeShifter = shapeShifter ?? throw new InvalidOperationException("shapeShifter is null");
        _mover = mover ?? throw new InvalidOperationException("mover is null");
        _runeDisplayer = runeDisplayer ?? throw new InvalidOperationException("runeDisplayer is null");
    }

    internal event Action<bool> ReleasedOnRestart;
    internal event Action<Vector3> ReturnedOnStartPosition;

    internal bool IsRaised { get; private set; } = false;
    internal bool IsBackStartPosition { get; private set; } = false;
    internal bool IsFrozen { get; private set; } = false;
    internal Vector3 StartPosition { get; private set; } = Vector3.zero;
    internal bool IsRelease => _cubes.Count == 0;
    internal int CubeCount => _cubes.Count;
    internal List<LocalPosition> CubePositions  => _cubePositions.ToList();

    public void ChangeCubeState(CubeState state)
    {
        if (state == null)
            throw new ArgumentNullException("state is null", nameof(state));

        if (state is FrozenState && state.Value)
            IsFrozen = true;

        foreach (var cube in _cubes)
            cube.ChangeState(state);
    }

    internal void Update()
    {
        if (IsRaised == false || IsBackStartPosition)
            return;

        _mover.Move(_transform, _verticalShift);
        _runeDisplayer.ChangeRunesDisplay(_positionHandler.GetCubePositions(_cubes));
    }

    internal void SetStartParametrs(Vector3 startPosition)
    {
        _transform.position = startPosition;
        StartPosition = startPosition;
        IsFrozen = false;
        IsRaised = false;
        IsBackStartPosition = false;

        ChangeCubeState(_defaultSizeState);
    }

    internal void SetRaisedState(Vector3 cubePosition)
    {
        IsRaised = true;
        IsBackStartPosition = false;
        _mover.CalculateOffset(cubePosition);

        ChangeCubeState(_transparencyState);
        ChangeCubeState(_defaultSizeState);
    }

    internal void Take(List<Cube> cubes)
    {      
        if (cubes == null)
            throw new ArgumentNullException("cubes is null", nameof(cubes));

        if (cubes.Count == 0)
            throw new InvalidOperationException("cubes is empty");

        foreach (var cube in cubes)
        {
            if (cube == null)
                throw new ArgumentNullException("cube is null", nameof(cube));

            _cubes.Add(cube);
            _cubePositions.Add(cube.LocalPosition);
        }

        _verticalShift = _shapeShifter.CalculateOffset(_cubePositions);
    }

    internal bool TryLand()
    {
        _runeDisplayer.DisableAllRunes();

        if (_shapeLander.TryLand(_cubes))
        {
            IsRaised = false;
            IsFrozen = false;
            Release(false);
            return true;
        }
        else
        {
            IsBackStartPosition = true;
            ReturnedOnStartPosition?.Invoke(StartPosition);
            return false;
        }
    }

    internal void ChangeToLanded()
    {
        ChangeCubeState(_smallSizeState);
        IsRaised = false;
    }

    internal void ReleaseOnRestart()
    {
        foreach (var cube in _cubes)
            cube.Restart();

        IsFrozen = false;
        Release(true);
    }

    private void Release(bool isRestart)
    {
        ReleasedOnRestart?.Invoke(isRestart);
        _cubes.Clear();
        _cubePositions.Clear();
    }
}