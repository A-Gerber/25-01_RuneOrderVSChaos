using System;

internal class AreaModelFactory
{
    internal AreaModel CreateArea(CellModel[,] cells)
    {
        if (cells.GetLength(0) == 0 || cells.GetLength(1) == 0)
            throw new InvalidOperationException("cells are not correct");

        AreaModel area = new AreaModel(cells);

        return area;
    }

    internal CellModel[,] CreateCells()
    {
        CellModel[,] cells = new CellModel[UserUtilities.AreaSize, UserUtilities.AreaSize];

        for (int x = UserUtilities.OriginByX; x < UserUtilities.AreaSize; x++)
        {
            for (int z = UserUtilities.OriginByZ; z < UserUtilities.AreaSize; z++)
            {
                cells[x, z] = new CellModel(new LocalPosition(x, z));
            }
        }

        return cells;
    }
}