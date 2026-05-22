using System;
using UnityEngine;

public class AreaView : MonoBehaviour, IRuneToggleable
{
    [SerializeField] private Transform _cellContainer;

    private AreaModel _area;

    private void Update()
    {
        _area.ChangeRuneDisplay();
    }

    public void Initialize(AreaModel area)
    {
        _area = area ?? throw new InvalidOperationException("area is null");
    }

    public void ChangeRuneState(bool isEnabled)
    {    }

    public Transform GetContainer()
    {
        return _cellContainer;
    }
}