using System;
using UnityEngine;

internal class ShapeLifter
{
    private Camera _camera;
    private Ray _ray;

    private ILiftable _shape;

    internal event Action Puted;

    public ShapeLifter(Camera camera, Ray ray)
    {
        if (camera == null)
            throw new InvalidOperationException("camera is null");

        _camera = camera;
        _ray = ray;
    }
    
    internal void LiftShape()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity) && hit.transform.TryGetComponent(out CubeView cube))
        {
            ILiftable shape = cube.GetLiftableShape();

            if (shape.IsRaised == false)
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
            Puted?.Invoke();
        }
    }
}