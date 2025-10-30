using System;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class ShapeMover
    {
        private float _height;
        private float _speed;

        internal ShapeMover(float height, float speed)
        {
            if (speed < 0)
                throw new ArgumentOutOfRangeException(nameof(speed));

            _height = height;
            _speed = speed;
        }

        internal void Move(Transform transform)
        {
            if (transform == null)
                throw new InvalidOperationException("transform is null");

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = _height;

            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
        }
    }
}