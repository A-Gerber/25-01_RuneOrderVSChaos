using System;
using UnityEngine;

namespace RuneOrderVSChaos
{
    [RequireComponent(typeof(Renderer))]
    internal class CellView : MonoBehaviour, IChangeableColor
    {
        private CellModel _cell;
        private Renderer _renderer;
        private ColorChanger _colorChanger;

        [field: SerializeField] public bool IsBusy { get; set; }
        //internal bool IsBusy => _cell.IsBusy;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _colorChanger = new ColorChanger(_renderer);    
        }

        private void Update()
        {
            IsBusy = _cell.IsBusy;
        }

        public void ChangeColorCells()
        {
            _cell.ChangeColor();
        }

        internal void Take(CubeModel cube)
        {
            _cell.Take(cube);
        }

        internal void SetDefaultColorCell()
        {
            _cell.SetDefaultColor();
        }

        internal void Initialize(CellModel cell) 
        {
            if(_cell != null)
                _cell.ChangedColor -= OnChangeColor;

            _cell = cell ?? throw new InvalidOperationException("cell is null");

            _cell.ChangedColor += OnChangeColor;
        }

        private void OnChangeColor(Color color)
        {
            _colorChanger.ChangeColor(color);
        }
    }
}