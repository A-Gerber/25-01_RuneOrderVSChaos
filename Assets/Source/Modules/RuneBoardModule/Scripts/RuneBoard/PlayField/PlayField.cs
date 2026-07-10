using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class PlayField : ICellGetable, IPlayFieldSkillContactable
{
    private readonly List<IReleasable> _targets = new();
    private readonly List<IReleasable> _pushTargets = new();
    private readonly List<LocalPosition> _projectilePositions = new();
    private readonly Cell[,] _cells;
    private readonly RuneDisplayer _runeDisplayer;
    private readonly Pusher _pusher;
    private readonly FinderFullLinesOfCells _finderFullLines = new();
    private readonly ShapePlatform _shapePlatform;
    private readonly FinderPlacesForShapes _finderPlaces;

    internal PlayField(Cell[,] cells, RuneDisplayer runeDisplayer, ShapePlatform shapePlatform, Pusher pusher)
    {
        if (cells.GetLength(0) == 0 || cells.GetLength(1) == 0)
            throw new ArgumentException("cells is not correct", nameof(cells));

        _runeDisplayer = runeDisplayer ?? throw new ArgumentNullException("runeDisplayer is null", nameof(runeDisplayer));
        _cells = cells ?? throw new ArgumentNullException("cells is null", nameof(cells));
        _shapePlatform = shapePlatform ?? throw new ArgumentNullException("shapePlatform is null", nameof(shapePlatform));
        _pusher = pusher ?? throw new ArgumentNullException("pusher is null", nameof(pusher));

        _finderPlaces = new FinderPlacesForShapes(cells);
    }

    internal ShapePlatform ShapePlatform => _shapePlatform;

    public bool TryIdentifyTargets(List<LocalPosition> coordinates, Vector3 forceImpactPosition)
    {
        _targets.Clear();
        _projectilePositions.Clear();
        _pushTargets.Clear();

        foreach ((Cell currentCell, _, _) in GetAllCells())
        {
            if (currentCell.IsBusy && coordinates.Any(coordinate => UserUtilities.IsEqualPosition(currentCell.Position, coordinate)))
                AddTarget(currentCell);
        }

        _pusher.Push(_pushTargets, forceImpactPosition);
        return _targets.Count > 0;
    }

    public bool TryGetCellByPosition(out ITakeable cell, LocalPosition localPosition)
    {
        cell = null;

        foreach (var (currentCell, i, j) in GetAllCells())
        {
            if (UserUtilities.IsEqualPosition(currentCell.Position, localPosition))
            {
                cell = currentCell;
                return true;
            }
        }

        return false;
    }

    internal bool TryReleaseTargets(out List<LocalPosition> targetPositions)
    {
        targetPositions = new List<LocalPosition>();

        if (_targets.Count == 0)
            return false;

        targetPositions.AddRange(_projectilePositions);

        foreach (var target in _targets)
            target.TryRelease();

        return true;
    }

    internal void Reset(int level)
    {
        _runeDisplayer.DisableAllRunes();
        _shapePlatform.Reset(level);

        foreach (var (cell, i, j) in GetAllCells().Where(c => c.cell.IsBusy))
        {
            cell.GetItem().Restart();
            cell.Release();
        }
    }

    internal bool IsLostGame()
    {
        for (int k = 0; k < _shapePlatform.ShapeCount; k++)
        {
            if (_shapePlatform.TryGetCubePositionsByIndex(out List<LocalPosition> positions, k))
            {
                List<LocalPosition> offsetPositions = _finderPlaces.ShiftPositionByOffset(positions, positions[0], false);

                bool hasFreePlace = GetAllCells().Any(cellTuple => _finderPlaces.IsCellsFreeForShape(offsetPositions, cellTuple.cell.Position));

                if (hasFreePlace)
                    return false;
            }
        }

        return true;
    }

    internal bool TryReleaseCellsByLines(out List<LocalPosition> targetPositions)
    {
        return _finderFullLines.TryReleaseCellsByLines(out targetPositions, _cells);
    }

    private void AddTarget(Cell currentCell)
    {
        IReleasable target = currentCell.GetItem();
        _targets.Add(target);

        if (target is Cube cube && cube.IsFrozen)
            return;

        _pushTargets.Add(target);
        _projectilePositions.Add(currentCell.Position);
        currentCell.Release();
    }

    private IEnumerable<(Cell cell, int i, int j)> GetAllCells()
    {
        for (int i = 0; i < _cells.GetLength(0); i++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                yield return (_cells[i, j], i, j);
            }
        }
    }
}