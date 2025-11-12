using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class FinderPlacesForShapes
    {
        private readonly CellModel[,] _playField;

        internal FinderPlacesForShapes(CellModel[,] playField)
        {
            _playField = playField ?? throw new InvalidOperationException("playField is null");
        }

        internal List<LocalPosition> ShiftPositionByOffset(List<LocalPosition> positions, LocalPosition offset, bool positiveShift)
        {
            List<LocalPosition> newPositions = new();

            int coefficient;

            if (positiveShift)
                coefficient = 1;
            else
                coefficient = -1;

            foreach (var position in positions)            
               newPositions.Add(new LocalPosition(position.PositionX + offset.PositionX * coefficient, position.PositionZ + offset.PositionZ * coefficient));         

            return newPositions;
        }

        internal bool IsCellsFreeForShape(List<LocalPosition> offsetPositions, LocalPosition cellPosition)
        {
            List<LocalPosition> cubePositionsInAreaCoordinates = ShiftPositionByOffset(offsetPositions, cellPosition, true);
            List<CellModel> checkCells = new List<CellModel>();

            for (int k = 0; k < cubePositionsInAreaCoordinates.Count; k++)
            {
                for (int i = 0; i < _playField.GetLength(0); i++)
                {
                    for (int j = 0; j < _playField.GetLength(1); j++)
                    {
                        if (IsEqualPosition(cubePositionsInAreaCoordinates[k], _playField[i, j].Position))                        
                            checkCells.Add(_playField[i, j]);                     
                    }
                }
            }

            if (IsCheckCellsBusy(checkCells, cubePositionsInAreaCoordinates.Count))            
                return false;          

            return true;
        }

        private bool IsCheckCellsBusy(List<CellModel> checkCells, int cubeCounInShape)
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

        private bool IsEqualPosition(LocalPosition firstPosition, LocalPosition secondPosition)
        {
            return firstPosition.PositionX == secondPosition.PositionX && firstPosition.PositionZ == secondPosition.PositionZ;
        }
    }
}