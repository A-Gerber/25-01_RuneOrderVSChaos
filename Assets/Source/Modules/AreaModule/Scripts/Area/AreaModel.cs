using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaModel : IUseableUserSkills, IUseableEnemySkills, IChangeableRuneDisplay
{
    private readonly CellModel[,] _playField;
    private readonly List<CellModel> _targetCells = new();
    private readonly FinderFullLinesOfCells _finderFullLines = new();
    private readonly FinderFullCellsInArea _finderInArea = new();
    private readonly FinderEmptyCell _finderEmptyCell = new();
    private readonly FinderPlacesForShapes _finderPlaces;
    private Shape[] _shapeModel;

    private int _index = 0;
    private bool _canChangeRuneDisplay = false;

    public AreaModel(CellModel[,] playField)
    {
        if (playField.GetLength(0) == 0 || playField.GetLength(1) == 0)
            throw new InvalidOperationException("cells are not correct");

        _playField = playField;
        _finderPlaces = new(_playField);
    }

    public int CountTargetDamage { get; private set; } = 0;
    public int CountTargets => _targetCells.Count;


    public void Initialize(Shape[] shapeModels)
    {
        _shapeModel = shapeModels ?? throw new InvalidOperationException("shapeModels is null");
    }

    public void TakeShapeModel(Shape shapeModel)
    {
        _shapeModel[_index] = shapeModel ?? throw new InvalidOperationException("shapeView is null");
        _index = ++_index % _shapeModel.Length;
    }

    public bool TryFindTargetCellsByLines()
    {
        if (_finderFullLines.TryGetFullCellsByLines(out List<CellModel> targetCells, _playField))
        {
            _targetCells.AddRange(targetCells);
            CountTargetDamage = _targetCells.Count;

            return true;
        }

        return false;
    }

    public bool TryFindTargetsForStrike(List<LocalPosition> coordinates, out List<Cube> targets)
    {
        targets = new List<Cube>();

        if (_finderInArea.TryGetBusyCellsByArea(out List<CellModel> targetCells, _playField, coordinates))
        {
            foreach (var cell in targetCells)
            {
                IReleaseable item = cell.GetItem();

                if (item is Cube cube)
                {
                    if (cube.IsFrozen)
                    {
                        item.Release();
                    }
                    else
                    {
                        _targetCells.Add(cell);
                        targets.Add(cube);
                    }
                }
                else
                {
                    _targetCells.Add(cell);
                }
            }

            CountTargetDamage = _targetCells.Count;
            return true;
        }

        return false;
    }

    public void SetCountTargetDamage(int count)
    {
        CountTargetDamage = count;
    }

    public List<CellModel> GetCellsForFilling(out List<LocalPosition> cellCoordinates, IReadOnlyList<LocalPosition> skillCoordinates)
    {
        return _finderEmptyCell.FindCellsForFilling(out cellCoordinates, skillCoordinates, _playField);
    }

    public void ReleaseTargetCubes()
    {
        foreach (var cell in _targetCells)
        {
            cell.GetItem().Release();
            cell.Release—ell();
        }

        _targetCells.Clear();
    }

    public void Restart()
    {
        for (int i = 0; i < _playField.GetLength(0); i++)
        {
            for (int j = 0; j < _playField.GetLength(1); j++)
            {
                if (_playField[i, j].IsBusy)
                    _playField[i, j].GetItemWhenRestarting().Restart();
            }
        }

        for (int i = 0; i < _shapeModel.Length; i++)
        {
            if (_shapeModel[i].IsRelease == false)
                _shapeModel[i].ReleaseOnRestart();
        }
    }

    public bool IsLostGame()
    {
        for (int k = 0; k < _shapeModel.Length; k++)
        {
            if (_shapeModel[k] != null && _shapeModel[k].IsRelease == false)
            {
                List<LocalPosition> offsetPositions = _shapeModel[k].GetLocalPositionCubes();
                offsetPositions = _finderPlaces.ShiftPositionByOffset(offsetPositions, offsetPositions[0], false);

                for (int i = 0; i < _playField.GetLength(0); i++)
                {
                    for (int j = 0; j < _playField.GetLength(1); j++)
                    {
                        if (_finderPlaces.IsCellsFreeForShape(offsetPositions, _playField[i, j].Position))
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    public List<Vector3> GetPositionTargetCells()
    {
        List<Vector3> position = new();

        foreach (var cell in _targetCells)
        {
            position.Add(new Vector3(cell.Position.PositionX, 0, cell.Position.PositionZ));
        }

        return position;
    }

    public bool TryGetCellByCoordinate(out ITakeable cell, LocalPosition position)
    {
        cell = null;

        for (int i = 0; i < _playField.GetLength(0); i++)
        {
            for (int j = 0; j < _playField.GetLength(1); j++)
            {
                if (UserUtilities.IsEqualPosition(position, new LocalPosition(i, j)))
                {
                    cell = _playField[i, j];
                    return true;
                }
            }
        }

        return false;
    }

    public bool TryFreezeRandomShape(ref Vector3 position)
    {
        int index = UnityEngine.Random.Range(0, _shapeModel.Length);
        bool canFreezeShape = false;

        for (int i = 0; i < _shapeModel.Length; i++)
        {
            if (_shapeModel[index].IsFrozen || _shapeModel[index].IsRelease)
            {
                index = ++index % _shapeModel.Length;
            }
            else
            {
                canFreezeShape = true;
                break;
            }
        }

        if (canFreezeShape)
        {
            if (_shapeModel[index].IsRaised)
                position = UserUtilities.GetCursorPosition(Constants.CameraHeight);
            else
                position = _shapeModel[index].StartPosition;

            _shapeModel[index].FreezeCubes();
            return true;
        }

        return false;
    }

    public void ChangeRuneDisplay()
    {
        if (_canChangeRuneDisplay)
        {
            for (int i = 0; i < _playField.GetLength(0); i++)
            {
                for (int j = 0; j < _playField.GetLength(1); j++)
                    _playField[i, j].ChangeRuneDisplay();
            }
        }
    }

    public void ChangeState(bool canChangeDisplay)
    {
        _canChangeRuneDisplay = canChangeDisplay;
    }

    internal void DisableRunes()
    {
        for (int i = 0; i < _playField.GetLength(0); i++)
        {
            for (int j = 0; j < _playField.GetLength(1); j++)
            {
                _playField[i, j].DisableRune();
            }
        }
    }
}

