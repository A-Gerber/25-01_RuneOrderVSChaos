using System;
using UnityEngine;

public class AreaView : MonoBehaviour, IChangeableColor
{
    [SerializeField] private Transform _cellContainer;

    private AreaModel _area;

    public void ChangeColorCells()
    {
        _area.ChangeColorCells();
    }

    public void Initialize(AreaModel area)
    {
        _area = area ?? throw new InvalidOperationException("area is null");
    }

    public Transform GetContainer()
    {
        return _cellContainer;
    }
}