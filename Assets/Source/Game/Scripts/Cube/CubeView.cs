using System;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class CubeView : MonoBehaviour
    {
        [SerializeField] private float _durationLanding = 0.5f;
        [SerializeField] private float _distanceRaycast = 5f;

        private CubeModel _cubeModel;

        internal event Action<CubeView> Released;

        internal Vector3 LocalPosition { get; private set; }
        internal float DurationLanding => _durationLanding;
        internal float DistanceRaycast => _distanceRaycast;

        internal void Initialize(CubeModel cube)
        {
            if (_cubeModel != null)
                _cubeModel.Released -= OnRelease;

            _cubeModel = cube ?? throw new InvalidOperationException("cube is null");

            _cubeModel.Released += OnRelease;
        }

        internal CubeModel GetCubeModel()
        {
            return _cubeModel;
        }

        internal void SetLocalPosition(LocalPosition position)
        {
            _cubeModel.SetLocalPosition(position);

            LocalPosition = new Vector3(position.PositionX, 0 , position.PositionZ);
        }

        private void OnRelease()
        {
            Released?.Invoke(this);
        }
    }
}