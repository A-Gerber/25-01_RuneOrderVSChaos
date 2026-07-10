using System;
using UnityEngine;

internal class ShapeLifter
{
    private readonly LayerMask _layerMask;
    private readonly Camera _camera;
    private readonly RaycastHit[] _results;

    private Ray _ray;
    private ILiftable _shape;

    internal ShapeLifter(Camera camera, Ray ray, LayerMask layerMask)
    {
        if (camera == null)
            throw new InvalidOperationException("camera is null");

        _camera = camera;
        _ray = ray;
        _results = new RaycastHit[3];
        _layerMask = layerMask;
    }

    internal void Put()
    {
        if (_shape != null)
        {
            _shape.Land();
            _shape = null;
        }
    }

    internal void Lift()
    {
        if (RayCastController.CanPerformRayCast == false)
            return;

        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        int hitCount = Physics.RaycastNonAlloc(_ray, _results, Mathf.Infinity, _layerMask);

        if (hitCount > 0 && TryGetRaisedShape(out ILiftable shape, hitCount))
        {           
            _shape = shape;
            _shape.SetStatusRaised(GetCubeLocalPosition(hitCount));
        }
    }

    private Vector3 GetCubeLocalPosition(int hitCount)
    {
        for (int i = 0; i < hitCount; i++)
        {
            if (_results[i].transform.TryGetComponent(out CubePresenter _))
                return _results[i].transform.localPosition;
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