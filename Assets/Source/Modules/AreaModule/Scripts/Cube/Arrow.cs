using System;
using System.Collections.Generic;
using UnityEngine;

public class Arrow
{
    private readonly Transform _transform;
    private readonly List<LocalPosition> _coordinates = new();
    private readonly ISmallCubeSpawner _smallCubeSpawner;
    private Vector3 _direction;
    private Vector3 _oldPosition = -Vector3.one;

    public Arrow(Vector3 direction, Transform transform, ISmallCubeSpawner smallCubeSpawner)
    {
        _direction = direction;
        _transform = transform != null ? transform : throw new InvalidOperationException("transform is null");
        _smallCubeSpawner = smallCubeSpawner ?? throw new InvalidOperationException("smallCubeSpawner is null");
    }

    public event Action<Arrow> Activating;
    internal event Action Destroyed;

    public IReadOnlyList<LocalPosition> Coordinates => _coordinates;
    public Vector3 Direction => _direction;

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
        if (Physics.Raycast(centr, _direction, out RaycastHit hit, Constants.AreaSize))
        {
            Vector3 endPointRaycast = new(Mathf.Round(hit.point.x), Constants.CellSize / Constants.HalfDivider, Mathf.Round(hit.point.z));

            for (int i = 1; i <= Constants.AreaSize; i++)
            {
                Vector3 vector = Vector3.zero;
                vector += centr + _direction * i;

                if (UserUtilities.IsLocateInArena(vector) && UserUtilities.IsEqualVector3(vector, endPointRaycast) == false)
                {
                    _smallCubeSpawner.Create(vector);
                    _coordinates.Add(new LocalPosition((int)vector.x, (int)vector.z));
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            for (int i = 1; i <= Constants.AreaSize; i++)
            {
                Vector3 vector = Vector3.zero;
                vector += centr + _direction * i;

                if (UserUtilities.IsLocateInArena(vector))
                {
                    _smallCubeSpawner.Create(vector);
                    _coordinates.Add(new LocalPosition((int)vector.x, (int)vector.z));
                }
            }
        }
    }

    private bool TryGetÑellÑenter(out Vector3 centr)
    {
        centr = new Vector3(Mathf.Round(_transform.position.x), Constants.CellSize / Constants.HalfDivider, Mathf.Round(_transform.position.z));
        Vector3 offset = _transform.position - centr;

        return Mathf.Abs(offset.x) <= Constants.CubeSize / Constants.HalfDivider || Mathf.Abs(offset.z) <= Constants.CubeSize / Constants.HalfDivider;
    }
}