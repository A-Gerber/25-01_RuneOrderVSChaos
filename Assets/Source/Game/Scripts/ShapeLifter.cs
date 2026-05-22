using System;
using UnityEngine;

internal class ShapeLifter
{
    private readonly LayerMask _layerMask;
    private readonly Camera _camera;
    private readonly RaycastHit[] _results;

    private Ray _ray;
    private ILiftable _shape;

    public ShapeLifter(Camera camera, Ray ray, LayerMask layerMask)
    {
        if (camera == null)
            throw new InvalidOperationException("camera is null");

        _camera = camera;
        _ray = ray;
        _results = new RaycastHit[3];
        _layerMask = layerMask;
    }

    internal void LiftShape1()
    {
        if (UserUtilities.CanPerformRaycast)
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity) && hit.transform.TryGetComponent(out ILiftable shape) && shape.IsRaised == false)
            {
                _shape = shape;
                //_shape.SetStatusRaised();
            }
        }
    }

    internal void PutShape()
    {
        if (_shape != null)
        {
            _shape.Put();
            _shape = null;
        }
    }

    internal void LiftShape()
    {
        if (UserUtilities.CanPerformRaycast)
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            int hitCount = Physics.RaycastNonAlloc(_ray, _results, Mathf.Infinity, _layerMask);

            if (hitCount > 0 && TryGetRaisedShape(out ILiftable shape, hitCount))
            {
                _shape = shape;
                _shape.SetStatusRaised(GetCubeTransform(hitCount));
            }
        }
    }

    private Vector3 GetCubeTransform(int hitCount)
    {
        for (int i = 0; i < hitCount; i++)
        {
            if (_results[i].transform.TryGetComponent(out CubeView cube))
                return cube.LocalPosition;
        }

        return Vector3.zero;
    }

    private bool TryGetRaisedShape(out ILiftable raisedShape, int hitCount)
    {
        raisedShape = null;

        for (int i = 0; i < hitCount; i++)
        {
            if (_results[i].transform.TryGetComponent(out ILiftable shape) && shape.IsRaised == false)
            {
                raisedShape = shape;
                return true;
            }
        }

        return false;
    }
}