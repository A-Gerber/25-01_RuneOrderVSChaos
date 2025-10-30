using System;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class AreaFactoryModel
    {
        internal AreaModel CreateArea(CellModel[,] cells)
        {
            if (cells.GetLength(0) == 0 || cells.GetLength(1) == 0)
                throw new InvalidOperationException("cells are not correct");   

            AreaModel area = new AreaModel(cells);

            return area;
        }

        internal CellModel[,] CreateCells(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            CellModel[,] cells = new CellModel[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    cells[x, y] = new CellModel( new Vector3(x, 0, y));
                    
                }
            }

            return cells;
        }
    }
}