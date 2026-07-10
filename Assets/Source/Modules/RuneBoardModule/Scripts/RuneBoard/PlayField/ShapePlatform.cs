using System;
using System.Collections.Generic;
using UnityEngine;

internal class ShapePlatform : IShapeFreezable
{
    private readonly Shape[] _shapes;
    private readonly List<Shape> _availableShapes = new();
    private readonly ShapePresenterSpawner _shapeSpawner;
    private readonly ConfigurationGenerator _configurationGenerator;

    private bool _isEnabled = true;
    private int _creationIndex;
    private int _index = 0;

    internal ShapePlatform(Shape[] shapes, ShapePresenterSpawner shapeSpawner, ConfigurationGenerator configurationGenerator)
    {
        if (shapes.Length == 0)
            throw new ArgumentException("shapeModels is not correct", nameof(shapes));

        _shapes = shapes ?? throw new ArgumentNullException("shapeModels is null", nameof(shapes));
        _shapeSpawner = shapeSpawner != null ? shapeSpawner : throw new ArgumentNullException("shapeSpawner is null", nameof(shapeSpawner));
        _configurationGenerator = configurationGenerator != null ? configurationGenerator : throw new ArgumentNullException("configurationGenerator is null", nameof(configurationGenerator));

        _shapeSpawner.Geted += Take;
    }

    internal int ShapeCount => _shapes.Length;

    public bool TryFreezeRandomShape(ref Vector3 position, FrozenState state)
    {
        _availableShapes.Clear();

        for (int i = 0; i < _shapes.Length; i++)
        {
            if (!_shapes[i].IsFrozen && !_shapes[i].IsRelease)
                _availableShapes.Add(_shapes[i]);
        }

        if (_availableShapes.Count == 0)
            return false;

        int index = UnityEngine.Random.Range(0, _availableShapes.Count);
        position = _availableShapes[index].IsRaised ? UserUtilities.GetCursorPosition(Constants.CameraHeight) : _availableShapes[index].StartPosition;

        _availableShapes[index].ChangeCubeState(state);

        return true;
    }

    internal void CreateShapes(int level)
    {
        if (++_creationIndex < Constants.ShapeCountForArea)
            return;

        for (int i = 0; i < Constants.ShapeCountForArea; i++)
            _shapeSpawner.CreateShape(_configurationGenerator.GetCubeConfigurator(level));

        _creationIndex = 0;
    }

    internal void Reset(int level)
    {
        for (int i = 0; i < _shapes.Length; i++)
        {
            if (_shapes[i] != null && _shapes[i].IsRelease == false)
                _shapes[i].ReleaseOnRestart();
        }

        _creationIndex = Constants.ShapeCountForArea;
        _configurationGenerator.ResetTimeCounter();
        CreateShapes(level);
    }

    internal void Set(bool isEnabled)
    {
        _isEnabled = isEnabled;
    }

    internal bool TryGetCubePositionsByIndex(out List<LocalPosition> positions, int k)
    {
        positions = new List<LocalPosition>();

        if (_shapes[k] == null || _shapes[k].IsRelease)
            return false;

        positions = _shapes[k].CubePositions;
        return true;
    }

    private void Take(Shape shapeModel)
    {
        if (_isEnabled == false)
            return;

        _shapes[_index] = shapeModel ?? throw new ArgumentNullException("shapeModel is null", nameof(shapeModel));
        _index = ++_index % _shapes.Length;
    }
}