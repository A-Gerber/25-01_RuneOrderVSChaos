using UnityEngine;

internal class AreaViewFactory : MonoBehaviour
{
    private const int Size = 8;
    private const int OriginByX = 0;
    private const int OriginByZ = 0;
    private const float HalfDivider = 2f;

    [SerializeField] private CellView _cellViewPrefab;
    [SerializeField] private AreaView _areaViewPrefab;

    private readonly AreaFactoryModel _areaFactoryModel = new AreaFactoryModel();
    private AreaModel _areaModel;
    private AreaView _areaView;
    private CellModel[,] _playField;

    internal float MinBorderArea => OriginByX - _cellViewPrefab.CellSize / HalfDivider;
    internal float MaxBorderArea => OriginByX + (Size - 1) + _cellViewPrefab.CellSize / HalfDivider;

    internal AreaModel Create()
    {
        _areaView = Instantiate(_areaViewPrefab, _areaViewPrefab.transform.position, Quaternion.identity);
        _playField = _areaFactoryModel.CreateCells(Size, OriginByX, OriginByZ);
        CreateCellViews();
        _areaModel = _areaFactoryModel.CreateArea(_playField);
        _areaView.Initialize(_areaModel);

        return _areaModel;
    }

    private void CreateCellViews()
    {
        for (int i = 0; i < _playField.GetLength(0); i++)
        {
            for (int j = 0; j < _playField.GetLength(1); j++)
            {
                Instantiate(_cellViewPrefab, new Vector3(_playField[i, j].Position.PositionX, 0, _playField[i, j].Position.PositionZ), Quaternion.identity, _areaView.GetContainer())
                    .Initialize(_playField[i, j]);
            }
        }
    }
}