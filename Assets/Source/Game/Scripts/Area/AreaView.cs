using System;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class AreaView : MonoBehaviour, IChangeableColor
    {
        [SerializeField] private Transform _cellContainer;
        [SerializeField] private float _timeBeforeAttack = 1.02f;

        private AreaModel _area;
        private float _countdown;

        private void Awake()
        {
            _countdown = _timeBeforeAttack;
        }

        private void FixedUpdate()
        {
            if(_area.IsCountdown)
            {
                if (_countdown <= 0)
                {
                    _area.AttackOverTime();
                    _countdown = _timeBeforeAttack;
                }

                _countdown -= Time.fixedDeltaTime;
            }
        }

        public void ChangeColorCell()
        {
            _area.ChangeColorCell();
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