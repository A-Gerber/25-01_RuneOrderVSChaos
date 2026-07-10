using System;
using System.Collections.Generic;

public class FinderPlacesForShapes 
{
    private readonly Cell[,] _cells;

    internal FinderPlacesForShapes(Cell[,] cells)
    {
        _cells = cells ?? throw new InvalidOperationException("cells is null");
    }

    internal List<LocalPosition> ShiftPositionByOffset(List<LocalPosition> positions, LocalPosition offset, bool positiveShift)
    {
        if (positions == null)
            throw new InvalidOperationException("positions is null");

        List<LocalPosition> newPositions = new();

        int coefficient;

        if (positiveShift)
            coefficient = 1;
        else
            coefficient = -1;

        foreach (var position in positions)
            newPositions.Add(new LocalPosition(position.X + offset.X * coefficient, position.Z + offset.Z * coefficient));

        return newPositions;
    }

    internal bool IsCellsFreeForShape(List<LocalPosition> offsetPositions, LocalPosition cellPosition)
    {
        if (offsetPositions == null)
            throw new InvalidOperationException("offsetPositions is null");

        List<LocalPosition> cubePositionsInAreaCoordinates = ShiftPositionByOffset(offsetPositions, cellPosition, true);
        List<Cell> checkCells = new ();

        for (int k = 0; k < cubePositionsInAreaCoordinates.Count; k++)
        {
            for (int i = 0; i < _cells.GetLength(0); i++)
            {
                for (int j = 0; j < _cells.GetLength(1); j++)
                {
                    if (UserUtilities.IsEqualPosition(cubePositionsInAreaCoordinates[k], _cells[i, j].Position))
                        checkCells.Add(_cells[i, j]);
                }
            }
        }

        if (IsCheckCellsBusy(checkCells, cubePositionsInAreaCoordinates.Count))
            return false;

        return true;
    }

    private bool IsCheckCellsBusy(List<Cell> checkCells, int cubeCounInShape)
    {
        if (checkCells.Count != cubeCounInShape)
        {
            return true;
        }

        foreach (var cell in checkCells)
        {
            if (cell.IsBusy)
                return true;
        }

        return false;
    }
}
