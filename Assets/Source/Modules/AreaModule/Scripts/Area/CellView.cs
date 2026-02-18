using System;
using UnityEngine;

public class CellView : MonoBehaviour, IDisplayChangeable
{
    [SerializeField] private ParticleSystem _rune;

    private CellModel _cell;

    internal bool IsBusy => _cell.IsBusy;

    public void ChangeDisplayRune()
    {
        _cell.EnableRune();
    }

    public void Initialize(CellModel cell)
    {
        if (_cell != null)
            _cell.ChangedDisplayRune -= OnChangeDisplayRune;

        _cell = cell ?? throw new InvalidOperationException("cell is null");

        _cell.ChangedDisplayRune += OnChangeDisplayRune;

        DisableRune();
    }

    internal void Take(IReleaseable item)
    {
        _cell.Take(item);
    }

    internal void DisableRune()
    {
        _cell.DisableRune();
    }

    private void OnChangeDisplayRune()
    {
        if(_cell.IsEnableRune)
            _rune.gameObject.SetActive(true);
        else 
            _rune.gameObject.SetActive(false);
    }
}