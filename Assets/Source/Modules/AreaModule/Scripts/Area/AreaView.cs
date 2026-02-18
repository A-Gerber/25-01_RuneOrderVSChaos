using System;
using UnityEngine;

public class AreaView : MonoBehaviour, IDisplayChangeable
{
    [SerializeField] private Transform _cellContainer;

    private AreaModel _area;

    public void ChangeDisplayRune()
    {
        _area.DisableRunes();
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