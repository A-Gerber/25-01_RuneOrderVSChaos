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
            _cubeModel = cube ?? throw new InvalidOperationException("cube is null");

            _cubeModel.Released += OnRelease;  // Подумать как отписаться

            Debug.Log("Подумать как отписаться");
        }

        internal CubeModel GetCubeModel()
        {
            return _cubeModel;
        }

        internal void SetLocalPosition(Vector3 position)
        {
            if (position == null)
                throw new InvalidOperationException("position is null");

            LocalPosition = position;
        }

        private void OnRelease()
        {
            Released?.Invoke(this);
        }
    }
}