using System;
using UnityEngine;

public class CellView : MonoBehaviour, IRuneToggleable
{
    [SerializeField] private ParticleSystem _rune;

    private CellModel _cell;

    internal bool IsBusy => _cell.IsBusy;

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    public void ChangeRuneState(bool isEnabled)
    {
        _cell.ChangeRuneState(isEnabled);
    }

    public void Initialize(CellModel cell)
    {
        Unsubscribe();

        _cell = cell ?? throw new InvalidOperationException("cell is null");

        Subscribe();
    }

    internal void Take(IReleaseable item)
    {
        _cell.Take(item);
    }

    private void OnChangeRuneDisplay()
    {
        _rune.gameObject.SetActive(_cell.IsEnabledRune);
    }

    private void Subscribe()
    {
        if (_cell != null)
            _cell.ChangedDisplayRune += OnChangeRuneDisplay;
    }

    private void Unsubscribe()
    {
        if (_cell != null)
            _cell.ChangedDisplayRune -= OnChangeRuneDisplay;
    }
}