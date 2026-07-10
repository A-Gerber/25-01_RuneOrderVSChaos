using System.Collections.Generic;
using UnityEngine;

public class CellFactory : MonoBehaviour
{
    private readonly Cell[,] _cells = new Cell[Constants.AreaSize, Constants.AreaSize];

    [SerializeField] private CellPresenter _cellViewPrefab;
    [SerializeField] private Transform _cellContainer;

    public void Create()
    {
        for (int x = Constants.OriginByX; x < Constants.AreaSize; x++)
        {
            for (int z = Constants.OriginByZ; z < Constants.AreaSize; z++)
            {
                LocalPosition position = new(x, z);
                _cells[x, z] = new Cell(position);
                Instantiate(_cellViewPrefab, new Vector3(position.X, 0, position.Z), Quaternion.identity, _cellContainer).Initialize(_cells[x, z]);
            }
        }
    }

    public List<Cell> GetListCells()
    {
        List<Cell> cells = new();

        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
                cells.Add(_cells[x, j]);
        }

        return cells;
    }

    public Cell[,] GetCells()
    {
        return _cells;
    }
}