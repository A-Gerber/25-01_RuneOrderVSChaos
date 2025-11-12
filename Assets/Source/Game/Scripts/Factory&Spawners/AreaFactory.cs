using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class AreaFactory : MonoBehaviour
    {
        private const int Size = 8;

        [SerializeField] private CellView _cellViewPrefab;
        [SerializeField] private AreaView _areaViewPrefab;
        
        private readonly AreaFactoryModel _areaFactoryModel = new AreaFactoryModel();
        private AreaModel _areaModel;
        private AreaView _areaView;
        private CellModel[,] _playField;


        internal AreaModel Create()
        {
            _areaView = Instantiate(_areaViewPrefab, _areaViewPrefab.transform.position, Quaternion.identity);
            _playField = _areaFactoryModel.CreateCells(Size);
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
                    Instantiate(_cellViewPrefab, new Vector3(_playField[i, j].Position.PositionX,0, _playField[i, j].Position.PositionZ), Quaternion.identity, _areaView.GetContainer())
                        .Initialize(_playField[i, j]);
                }
            }
        }
    }
}