using System;
using System.Collections.Generic;

internal class RuneDisplayer : IDisplayChangeable
{
    private readonly IReadOnlyList<Cell> _cells;
    private readonly HashSet<Cell> _enabledCells = new();
    private readonly HashSet<Cell> _tempCells = new();

    public RuneDisplayer(IReadOnlyList<Cell> cells)
    {
        if (cells.Count == 0)
            throw new ArgumentException("cells is empty", nameof(cells));

        _cells = cells ?? throw new ArgumentNullException(nameof(cells));
    }

    public void DisableAllRunes()
    {
        _enabledCells.Clear();

        foreach (Cell cell in _cells)
            cell.ChangeRuneDisplay(false);
    }

    public void ChangeRunesDisplay(List<LocalPosition> cubePositions)
    {
        if (cubePositions.Count == 0)
        {
            if (_enabledCells.Count > 0)
                DisableAllRunes();

            return;
        }

        UpdateEnabledCells(cubePositions);
    }

    private void UpdateEnabledCells(List<LocalPosition> cubePositions)
    {
        _tempCells.Clear();

        foreach (var cell in _cells)
        {
            if (cell.IsBusy)
                continue;

            foreach (var position in cubePositions)
            {
                if (UserUtilities.IsEqualPosition(position, cell.Position))
                {
                    _tempCells.Add(cell);
                    break;
                }
            }
        }

        if (AreSetsEqual(_enabledCells, _tempCells))
            return;

        foreach (var cell in _enabledCells)
            cell.ChangeRuneDisplay(false);

        foreach (var cell in _tempCells)
                cell.ChangeRuneDisplay(true);

        _enabledCells.Clear();
        _enabledCells.UnionWith(_tempCells);
    }

    private bool AreSetsEqual(HashSet<Cell> firstSet, HashSet<Cell> secondSet)
    {
        if (firstSet.Count != secondSet.Count)
            return false;

        return firstSet.SetEquals(secondSet);
    }
}