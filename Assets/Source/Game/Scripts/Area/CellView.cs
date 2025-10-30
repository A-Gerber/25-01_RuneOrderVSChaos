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

        public bool IsBusy => _cell.IsBusy;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _colorChanger = new ColorChanger(_renderer);
        }

        public void ChangeColorCell()
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
            _cell = cell ?? throw new InvalidOperationException("cell is null");

            _cell.ChangedColor += OnChangeColor; // Подумать как отписаться

            Debug.Log("Подумать как отписаться");
        }

        private void OnChangeColor(Color color)
        {
            _colorChanger.ChangeColor(color);
        }
    }
}