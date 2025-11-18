using System;
using UnityEngine;

public class CellModel
{
    private Color _defaultColor = Color.gray;
    private Color _selectColor = Color.cyan;

    private CubeModel _cube;
    private bool _isBusy = false;
    private LocalPosition _position;
    private Color _currentColor;

    public CellModel(LocalPosition position)
    {
        _position = position;
    }

    internal event Action<Color> ChangedColor;

    public LocalPosition Position => _position;

    internal bool IsBusy => _isBusy;

    internal void ChangeColor()
    {
        if (_currentColor != _selectColor)
        {
            _currentColor = _selectColor;
            ChangedColor?.Invoke(_currentColor);
        }
    }

    internal void SetDefaultColor()
    {
        if (_currentColor != _defaultColor)
        {
            _currentColor = _defaultColor;
            ChangedColor?.Invoke(_currentColor);
        }
    }

    internal void Take(CubeModel cube)
    {
        _cube = cube ?? throw new InvalidOperationException("cube is null");
        _isBusy = true;
    }

    internal CubeModel GetCube()
    {
        _isBusy = false;

        return _cube;
    }
}