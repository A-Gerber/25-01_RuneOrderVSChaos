using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CellView : MonoBehaviour, IChangeableColor
{
    private CellModel _cell;
    private Renderer _renderer;
    private ColorChanger _colorChanger;
    private Transform _transform;

    public float CellSize => transform.localScale.x;
    internal bool IsBusy => _cell.IsBusy;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _colorChanger = new ColorChanger(_renderer);
        _transform = transform;
    }

    public void ChangeColorCells()
    {
        _cell.ChangeColor();
    }

    public void Initialize(CellModel cell)
    {
        if (_cell != null)
            _cell.ChangedColor -= OnChangeColor;

        _cell = cell ?? throw new InvalidOperationException("cell is null");

        _cell.ChangedColor += OnChangeColor;
    }

    internal void Take(CubeModel cube)
    {
        _cell.Take(cube);
    }

    internal void SetDefaultColorCell()
    {
        _cell.SetDefaultColor();
    }

    private void OnChangeColor(Color color)
    {
        _colorChanger.ChangeColor(color);
    }
}