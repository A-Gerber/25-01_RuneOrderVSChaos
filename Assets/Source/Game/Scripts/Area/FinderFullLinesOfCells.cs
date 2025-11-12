using System.Collections.Generic;

namespace RuneOrderVSChaos
{
    internal class FinderFullLinesOfCells
    {
        internal bool TryGetFullCells(out List<CellModel> tempCells, CellModel[,] playField)
        {
            tempCells = CheckLineCells(true, playField);
            List<CellModel> horizontalCells = CheckLineCells(false, playField);

            foreach (var cell in tempCells)
            {
                if (horizontalCells.Contains(cell))
                    horizontalCells.Remove(cell);
            }

            tempCells.AddRange(horizontalCells);

            if (tempCells.Count > 0)
                return true;          

            return false;
        }

        private List<CellModel> CheckLineCells(bool isVertical, CellModel[,] playField)
        {
            List<CellModel> cellModels = new();
            List<CellModel> tempCells = new();

            for (int i = 0; i < playField.GetLength(0); i++)
            {
                bool isBusyLine = true;

                for (int j = 0; j < playField.GetLength(1); j++)
                {
                    if (isVertical)
                    {
                        tempCells.Add(playField[i, j]);

                        if (playField[i, j].IsBusy == false)
                            isBusyLine = false;
                    }
                    else
                    {
                        tempCells.Add(playField[j, i]);

                        if (playField[j, i].IsBusy == false)
                            isBusyLine = false;
                    }
                }

                if (isBusyLine)
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
}