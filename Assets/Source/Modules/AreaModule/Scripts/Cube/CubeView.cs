using System;
using UnityEngine;

public class CubeView : MonoBehaviour
{
    [SerializeField] private float _durationLanding = 0.5f;
    [SerializeField] private float _distanceRaycast = 5f;

    private CubeModel _cubeModel;

    public event Action<CubeView> Released;

    public float DurationLanding => _durationLanding;
    public float DistanceRaycast => _distanceRaycast;
    internal Vector3 LocalPosition { get; private set; }

    public void Initialize(CubeModel cube)
    {
        if (_cubeModel != null)
            _cubeModel.Released -= OnRelease;

        _cubeModel = cube ?? throw new InvalidOperationException("cube is null");

        _cubeModel.Released += OnRelease;
    }

    public void SetLocalPosition(LocalPosition position)
    {
        _cubeModel.SetLocalPosition(position);

        LocalPosition = new Vector3(position.PositionX, 0, position.PositionZ);
    }

    internal CubeModel GetCubeModel()
    {
        return _cubeModel;
    }

    private void OnRelease()
    {
        Released?.Invoke(this);
    }
}