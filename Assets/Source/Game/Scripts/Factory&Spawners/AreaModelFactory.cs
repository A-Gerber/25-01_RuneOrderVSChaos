using System;

internal class AreaModelFactory
{
    internal AreaModel CreateArea(CellModel[,] cells)
    {
        if (cells.GetLength(0) == 0 || cells.GetLength(1) == 0)
            throw new InvalidOperationException("cells are not correct");

        AreaModel area = new (cells);

        return area;
    }

    internal CellModel[,] CreateCells()
    {
        CellModel[,] cells = new CellModel[Constants.AreaSize, Constants.AreaSize];

        for (int x = Constants.OriginByX; x < Constants.AreaSize; x++)
        {
            for (int z = Constants.OriginByZ; z < Constants.AreaSize; z++)
            {
                cells[x, z] = new CellModel(new LocalPosition(x, z));
            }
        }

        return cells;
    }
}