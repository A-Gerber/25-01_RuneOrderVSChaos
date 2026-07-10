using System;
using System.Collections.Generic;

internal class FinderFullLinesOfCells
{
    HashSet<Cell> _verticalCells = new();
    HashSet<Cell> _horizontalCells = new();

    internal bool TryReleaseCellsByLines(out List<LocalPosition> positions, Cell[,] cells)
    {
        if (cells.GetLength(0) == 0 || cells.GetLength(1) == 0)
            throw new ArgumentException("cells is not correct", nameof(cells));

        if (cells == null)
            throw new ArgumentNullException("cells is null", nameof(cells));

        ClearSets();

        positions = new List<LocalPosition>();
        _verticalCells = CheckLineCells(true, cells);
        _horizontalCells = CheckLineCells(false, cells);

        foreach (var cell in _verticalCells)
        {
            if (_horizontalCells.Contains(cell))
                _horizontalCells.Remove(cell);
        }

        _verticalCells.UnionWith(_horizontalCells);

        if (_verticalCells.Count == 0)
            return false;

        foreach (var cell in _verticalCells)
        {
            if (cell.GetItem().TryRelease())
            {
                positions.Add(cell.Position);
                cell.Release();
            }
        }

        return true;
    }

    private HashSet<Cell> CheckLineCells(bool isVertical, Cell[,] cells)
    {
        HashSet<Cell> cellModels = new();
        HashSet<Cell> temp = new();

        for (int i = 0; i < cells.GetLength(0); i++)
        {
            bool isBusyLineByCubes = true;

            for (int j = 0; j < cells.GetLength(1); j++)
            {
                if (isVertical)
                {
                    temp.Add(cells[i, j]);

                    if (cells[i, j].IsBusy == false || cells[i, j].GetItem() is Stalactite)
                    {
                        isBusyLineByCubes = false;
                        break;
                    }
                }
                else
                {
                    temp.Add(cells[j, i]);

                    if (cells[j, i].IsBusy == false || cells[j, i].GetItem() is Stalactite)
                    {
                        isBusyLineByCubes = false;
                        break;
                    }
                }
            }

            if (isBusyLineByCubes)
                cellModels.UnionWith(temp);

            temp.Clear();
        }

        return cellModels;
    }

    private void ClearSets()
    {
        _verticalCells.Clear();
        _horizontalCells.Clear();
    }
}