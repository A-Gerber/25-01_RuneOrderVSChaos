using System;
using UnityEngine;

internal class ShapeLifter
{
    private readonly Camera _camera;
    private Ray _ray;
    private ILiftable _shape;

    internal event Action Placed;

    public ShapeLifter(Camera camera, Ray ray)
    {
        if (camera == null)
            throw new InvalidOperationException("camera is null");

        _camera = camera;
        _ray = ray;
    }

    internal void LiftShape()
    {
        if (UserUtilities.CanPerformRaycast)
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity) && hit.transform.TryGetComponent(out ILiftable shape) && shape.IsRaised == false)
            {
                _shape = shape;
                _shape.SetStatusRaised();
            }
        }
    }

    internal void PutShape()
    {
        if (_shape != null)
        {
            _shape.Put();
            _shape = null;
            Placed?.Invoke();
        }
    }
}