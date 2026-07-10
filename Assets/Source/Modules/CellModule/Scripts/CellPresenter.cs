using System;
using UnityEngine;

public class CellPresenter : MonoBehaviour
{
    [SerializeField] private ParticleSystem _rune;

    private Cell _cell;

    public bool IsBusy => _cell.IsBusy;

    public void Initialize(Cell cell)
    {
        if (_cell != null)
            _cell.ChangedDisplayRune -= OnChangeRuneDisplay;

        _cell = cell ?? throw new ArgumentNullException("cell is null", nameof(cell));

        if (_cell != null)
            _cell.ChangedDisplayRune += OnChangeRuneDisplay;
    }

    public void Take(IReleasable cube)
    {
        _cell.Take(cube);
    }

    private void OnChangeRuneDisplay(bool value)
    {
        if (enabled)
            _rune.gameObject.SetActive(value);
    }
}
