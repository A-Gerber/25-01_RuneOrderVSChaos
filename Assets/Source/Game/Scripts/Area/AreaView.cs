using System;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class AreaView : MonoBehaviour, IChangeableColor
    {
        [SerializeField] private Transform _cellContainer;

        private AreaModel _area;

        public void ChangeColorCells()
        {
            _area.ChangeColorCells();
        }

        internal void Initialize(AreaModel area)
        {
            _area = area ?? throw new InvalidOperationException("area is null");
        }

        internal Transform GetContainer()
        {
            return _cellContainer;
        }
    }
}