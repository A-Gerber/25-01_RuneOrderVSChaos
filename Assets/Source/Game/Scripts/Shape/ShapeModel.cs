using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class ShapeModel
    {
        private readonly List<CubeModel> _cubeModels = new();
        private readonly MoverTo _moverTo;
        private readonly ShapeMover _mover;
        private readonly Transform _transform;
        private readonly float _durationOfReturn;
        private Vector3 _startPosition;

        public ShapeModel(ShapeMover mover, Transform transform, float durationOfReturn)
        {
            _mover = mover ?? throw new InvalidOperationException("mover is null");
            _transform = transform != null ? transform : throw new InvalidOperationException("transform is null");

            if(durationOfReturn <= 0)
                throw new ArgumentOutOfRangeException(nameof(durationOfReturn));

            _durationOfReturn = durationOfReturn;
            _moverTo = new MoverTo(_transform);
        }

        internal event Action Released;

        internal bool IsRaised { get; private set; } = false;

        internal void SetStartPosition(Vector3 startPosition)
        {
            if (startPosition == null)
                throw new InvalidOperationException("startPosition is null");

            _startPosition = startPosition;
        }

        internal void TakeCubes(List<CubeModel> cubeModels)
        {
            foreach (var cube in cubeModels)
            {
                _cubeModels.Add(cube);
            }
        }

        internal void RemoveCubes()
        {
            _cubeModels.Clear();
        }

        internal void SetRaise(bool value)
        {
            IsRaised = value;
        }

        internal void Raise()
        {
            _mover.Move(_transform);

            foreach (var cubeModel in _cubeModels)
            {
                cubeModel.TrackLanding();
            }
        }

        internal void Put()
        {
            if (IsFreeSpace()) 
            {
                foreach (var cubeModel in _cubeModels)
                {
                    cubeModel.Land();
                }

                Released?.Invoke();
            }
            else
            {
                _moverTo.MoveTo(_startPosition, _durationOfReturn);
            }
        }

        private bool IsFreeSpace()
        {
            bool isFreeSpace = true;

            foreach (var cubeModel in _cubeModels)
            {
                if(cubeModel.TryGetBusyCell())
                    isFreeSpace = false;
            }

            return isFreeSpace;
        }
    }
}