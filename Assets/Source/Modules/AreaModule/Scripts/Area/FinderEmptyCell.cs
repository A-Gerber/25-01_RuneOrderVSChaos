using System;
using System.Collections.Generic;

internal class FinderEmptyCell
{
    internal List<CellModel> FindCellsForFilling(out List<LocalPosition> cellCoordinates, List<LocalPosition> skillCoordinates, CellModel[,] playField)
    {
        List<CellModel> targetCells = new();
        cellCoordinates = new();

        foreach (var coordinate in skillCoordinates)
        {
            for (int i = 0; i < playField.GetLength(0); i++)
            {
                for (int j = 0; j < playField.GetLength(1); j++)
                {
                    if (LocalPositionsComparator.IsEqualPosition(coordinate, playField[i, j].Position) && playField[i, j].IsBusy == false)
                    {
                        targetCells.Add(playField[i, j]);
                        cellCoordinates.Add(coordinate);
                    }                    
                }
            }
        }


        return targetCells;
    }
}