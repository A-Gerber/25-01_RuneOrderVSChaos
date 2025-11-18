using System.Collections.Generic;

internal class FinderFullCellsInArea
{
    internal bool TryGetFullCellsByArea(out List<CellModel> targetCells, CellModel[,] playField, List<LocalPosition> coordinates)
    {
        targetCells = new List<CellModel>();
        bool hasBusyCell = false;

        foreach (var coordinate in coordinates)
        {
            for (int i = 0; i < playField.GetLength(0); i++)
            {
                for (int j = 0; j < playField.GetLength(1); j++)
                {
                    if (LocalPositionsComparator.IsEqualPosition(coordinate, playField[i, j].Position) && playField[i, j].IsBusy)
                    {
                        targetCells.Add(playField[i, j]);
                        hasBusyCell = true;
                    }
                }
            }
        }

        return hasBusyCell;
    }
}