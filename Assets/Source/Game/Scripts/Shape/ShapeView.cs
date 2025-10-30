using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class ShapeView : MonoBehaviour
    {
        [SerializeField] private Transform _cubeContainer;
        [SerializeField] private float _height = 14f;
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _durationOfReturn = 1f;

        private ShapeModel _shapeModel;
        private ShapeMover _mover;

        internal event Action<ShapeView> Released;

        internal bool IsRaised => _shapeModel.IsRaised;
        internal float DurationOfReturn => _durationOfReturn;

        private void Awake()
        {
            _mover = new ShapeMover(_height, _speed);
        }

        private void Update()
        {
            if (IsRaised)
            {
                _shapeModel.Raise();
            }
        }

        internal void Initialize(ShapeModel shape)
        {
            _shapeModel = shape ?? throw new InvalidOperationException("shape is null");

            _shapeModel.Released += OnRelease;  // Подумать как отписаться

            Debug.Log("Подумать как отписаться");
        }

        internal void SetStartPosition(Vector3 startPosition)
        {
            _shapeModel.SetStartPosition(startPosition);
        }

        internal ShapeMover GetMover()
        {
            return _mover;
        }

        internal void TakeCubes(List<CubeView> cubeViews)
        {
            List<CubeModel> cubeModels = new List<CubeModel>();

            foreach (var cubeView in cubeViews)
            {
                cubeView.transform.parent = _cubeContainer;
                cubeView.transform.localPosition = cubeView.LocalPosition;
                cubeModels.Add(cubeView.GetCubeModel());
            }

            _shapeModel.TakeCubes(cubeModels);
        }

        internal void RemoveCubes()
        {
            _shapeModel.RemoveCubes();
        }

        internal void Raise()
        {
            _shapeModel.SetRaise(true);
        }

        internal void Put()
        {
            _shapeModel.SetRaise(false);
            _shapeModel.Put();
        }

        private void OnRelease()
        {
            Released?.Invoke(this);
        }
    }
}