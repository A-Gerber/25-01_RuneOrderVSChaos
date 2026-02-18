using System;
using System.Collections.Generic;
using UnityEngine;

internal class FinderFullLinesOfCells
{
    internal bool TryGetFullCellsByLines(out List<CellModel> NonFrozenCells, CellModel[,] playField)
    {
        if (playField == null)
            throw new InvalidOperationException("playField is null");

        NonFrozenCells = new();
        List<CellModel> tempCells = CheckLineCells(true, playField);
        List<CellModel> horizontalCells = CheckLineCells(false, playField);

        foreach (var cell in tempCells)
        {
            if (horizontalCells.Contains(cell))
                horizontalCells.Remove(cell);
        }

        tempCells.AddRange(horizontalCells);

        if (tempCells.Count == 0)
            return false;

        foreach (var cell in tempCells)
        {
            IReleaseable item = cell.GetItem();

            if (item is Cube cube && cube.IsFrozen)
                cube.Release();
            else
                NonFrozenCells.Add(cell);
        }

        if (NonFrozenCells.Count > 0)                    
            return true;
        
        return false;
    }

    private List<CellModel> CheckLineCells(bool isVertical, CellModel[,] playField)
    {
        List<CellModel> cellModels = new();
        List<CellModel> tempCells = new();

        for (int i = 0; i < playField.GetLength(0); i++)
        {
            bool isBusyLineByCubes = true;

            for (int j = 0; j < playField.GetLength(1); j++)
            {
                if (isVertical)
                {
                    tempCells.Add(playField[i, j]);

                    if (playField[i, j].IsBusy == false || playField[i, j].IsBusyByStalactite)
                        isBusyLineByCubes = false;
                }
                else
                {
                    tempCells.Add(playField[j, i]);

                    if (playField[j, i].IsBusy == false || playField[j, i].IsBusyByStalactite)
                        isBusyLineByCubes = false;
                }
            }

            if (isBusyLineByCubes)
            {
                cellModels.AddRange(tempCells);
                tempCells.Clear();
            }
            else
            {
                tempCells.Clear();
            }
        }

        return cellModels;
    }
}