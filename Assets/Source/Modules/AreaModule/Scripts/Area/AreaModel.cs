using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaModel
{
    private readonly CellModel[,] _playField;
    private readonly List<CellModel> _targetCells = new();
    private readonly FinderFullLinesOfCells _finderFullLines = new();
    private readonly FinderFullCellsInArea _finderInArea = new();
    private readonly FinderPlacesForShapes _finderPlaces;
    private ShapeModel[] _shapeModel;

    private int _index = 0;

    public AreaModel(CellModel[,] playField)
    {
        if (playField.GetLength(0) == 0 || playField.GetLength(1) == 0)
            throw new InvalidOperationException("cells are not correct");

        _playField = playField;
        _finderPlaces = new(_playField);
    }

    public int CountTargetCells => _targetCells.Count;

    public void Initialize(ShapeModel[] shapeModels)
    {
        _shapeModel = shapeModels ?? throw new InvalidOperationException("shapeModels is null");
    }

    public void TakeShapeModel(ShapeModel shapeModel)
    {
        if (shapeModel == null)
            throw new InvalidOperationException("shapeView is null");

        _shapeModel[_index] = shapeModel;
        _index = ++_index % _shapeModel.Length;

    }

    public bool TryFindTargetCellsByLines()
    {
        if (_finderFullLines.TryGetFullCellsByLines(out List<CellModel> targetCells, _playField))
        {
            _targetCells.AddRange(targetCells);

            return true;
        }

        return false;
    }

    public bool TryFindTargetCellsByCoordinates(List<LocalPosition> coordinates)
    {
        if (_finderInArea.TryGetFullCellsByArea(out List<CellModel> targetCells, _playField, coordinates))
        {
            _targetCells.AddRange(targetCells);

            return true;
        }

        return false;
    }

    public void ReleaseTargetCubes()
    {
        foreach (var cell in _targetCells)
        {
            cell.GetCube().Release();
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
                    _playField[i, j].GetCube().Release();
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

    internal void ChangeColorCells()
    {
        for (int i = 0; i < _playField.GetLength(0); i++)
        {
            for (int j = 0; j < _playField.GetLength(1); j++)
            {
                _playField[i, j].SetDefaultColor();
            }
        }
    }
}