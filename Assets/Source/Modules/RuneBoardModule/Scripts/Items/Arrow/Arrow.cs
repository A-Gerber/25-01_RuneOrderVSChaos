using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class Arrow
{
    private readonly Transform _transform;
    private readonly List<LocalPosition> _coordinates = new();
    private readonly SmallCubeSpawner _smallCubeSpawner;
    private Vector3 _direction;
    private Vector3 _oldPosition = -Vector3.one;

    public Arrow(Vector3 direction, Transform transform, SmallCubeSpawner smallCubeSpawner)
    {
        _direction = direction;
        _transform = transform != null ? transform : throw new ArgumentNullException("transform is null", nameof(transform));
        _smallCubeSpawner = smallCubeSpawner != null ? smallCubeSpawner : throw new ArgumentNullException("smallCubeSpawner is null", nameof(smallCubeSpawner));
    }

    internal event Action<Arrow> Activating;
    internal event Action Destroyed;

    internal Vector3 Direction => _direction;
    internal List<LocalPosition> CubePositions => _coordinates.ToList();

    internal void Destroy()
    {
        Destroyed?.Invoke();
    }

    internal void Activate()
    {
        Activating?.Invoke(this);

        _smallCubeSpawner.Release();
        _coordinates.Clear();
    }

    internal void TrackMovement()
    {
        if (TryGetÑellÑenter(out Vector3 centr) && UserUtilities.IsLocateInArena(centr))
        {
            if (Mathf.Approximately(centr.magnitude, _oldPosition.magnitude) == false)
            {
                Clear();
                GetTargetPositions(centr);
            }

            _oldPosition = centr;
        }
        else
        {
            Clear();
            _oldPosition = -Vector3.one;
        }
    }

    internal void Clear()
    {
        _smallCubeSpawner.Release();
        _coordinates.Clear();
    }

    private void GetTargetPositions(Vector3 centr)
    {
        Vector3 endPointRaycast = -Vector3.one;
        bool hasHit = Physics.Raycast(centr, _direction, out RaycastHit hit, Constants.AreaSize);

        if (hasHit) 
            endPointRaycast = new(Mathf.Round(hit.point.x), Constants.CellSize / Constants.HalfDivider, Mathf.Round(hit.point.z));

        for (int i = 1; i <= Constants.AreaSize; i++)
        {
            Vector3 position = centr + _direction * i;

            if (!UserUtilities.IsLocateInArena(position))
                continue;

            if (hasHit && UserUtilities.IsEqualVector3(position, endPointRaycast))
                break;

            _smallCubeSpawner.Create(position);
            _coordinates.Add(new LocalPosition((int)position.x, (int)position.z));
        }
    }


    private bool TryGetÑellÑenter(out Vector3 centr)
    {
        centr = new Vector3(Mathf.Round(_transform.position.x), Constants.CellSize / Constants.HalfDivider, Mathf.Round(_transform.position.z));
        Vector3 offset = _transform.position - centr;

        return Mathf.Abs(offset.x) <= Constants.CubeSize / Constants.HalfDivider || Mathf.Abs(offset.z) <= Constants.CubeSize / Constants.HalfDivider;
    }
}