using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class AreaModel
    {
        private readonly CellModel[,] _playField;
        private IAttackable _game;
        private readonly List<CellModel> _cellsWithTarget = new();

        internal AreaModel(CellModel[,] playField)
        {
            if (playField.GetLength(0) == 0 || playField.GetLength(1) == 0)
                throw new InvalidOperationException("cells are not correct");

            _playField = playField;
        }

        internal bool IsCountdown { get; private set; } = false;

        internal void Initialize(IAttackable game)
        {
            _game = game ?? throw new InvalidOperationException("game is null");
        }

        internal void ChangeColorCell()
        {
            for (int i = 0; i < _playField.GetLength(0); i++)
            {
                for (int j = 0; j < _playField.GetLength(1); j++)
                {
                    _playField[i, j].SetDefaultColor();
                }
            }
        }

        internal void AttackOverTime()
        {
            foreach (var cell in _cellsWithTarget)
            {
                cell.GetCube().Release();
            }

            _game.Attack(_cellsWithTarget.Count);
            _cellsWithTarget.Clear();
            IsCountdown = false;
        }

        internal void CheckArea()
        {
            List<CellModel> horizontalCells = CheckLineCells(false);
            List<CellModel> verticalCells = CheckLineCells(true);

            foreach (var cell in verticalCells)
            {
                if (horizontalCells.Contains(cell))
                    horizontalCells.Remove(cell);
            }

            verticalCells.AddRange(horizontalCells);


            if (verticalCells.Count > 0)
            {
                _cellsWithTarget.AddRange(verticalCells);
                IsCountdown = true;
            }
        }

        private List<CellModel> CheckLineCells(bool isVertical)
        {
            List<CellModel> cellModels = new();
            List<CellModel> tempCells = new();

            for (int i = 0; i < _playField.GetLength(0); i++)
            {
                bool isBusyLine = true;

                for (int j = 0; j < _playField.GetLength(1); j++)
                {
                    if (isVertical)
                    {
                        tempCells.Add(_playField[i, j]);

                        if (_playField[i, j].IsBusy == false)
                            isBusyLine = false;
                    }
                    else
                    {
                        tempCells.Add(_playField[j, i]);

                        if (_playField[j, i].IsBusy == false)
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