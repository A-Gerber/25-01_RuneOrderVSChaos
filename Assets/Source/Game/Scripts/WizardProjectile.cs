using System;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class WizardProjectile : MonoBehaviour
    {
        private Transform _transform;
        private Vector3 _target;
        private bool _canMove;
        private float _speed;

        internal event Action<WizardProjectile> Released;

        internal int Damage { get; private set; }

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            if (_canMove)
            {
                _transform.Translate(_speed * Time.deltaTime * (_target - _transform.position).normalized, Space.World);
            }
        }

        internal void Initialize(int damage, float speed, Vector3 target)
        {
            Damage = damage;
            _target = target;
            _speed = speed;
            _canMove = true;
        }

        internal void Release()
        {
            _canMove = false;

            Released?.Invoke(this);
        }
    }
}