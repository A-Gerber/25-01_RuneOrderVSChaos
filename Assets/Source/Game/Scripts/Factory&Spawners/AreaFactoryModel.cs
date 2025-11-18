using System;

internal class AreaFactoryModel
{
    internal AreaModel CreateArea(CellModel[,] cells)
    {
        if (cells.GetLength(0) == 0 || cells.GetLength(1) == 0)
            throw new InvalidOperationException("cells are not correct");

        AreaModel area = new AreaModel(cells);

        return area;
    }

    internal CellModel[,] CreateCells(int size, int originByX, int originByZ)
    {
        if (size <= 0)
            throw new ArgumentOutOfRangeException(nameof(size));

        CellModel[,] cells = new CellModel[size, size];

        for (int x = originByX; x < size; x++)
        {
            for (int z = originByZ; z < size; z++)
            {
                cells[x, z] = new CellModel(new LocalPosition(x, z));
            }
        }

        return cells;
    }
}