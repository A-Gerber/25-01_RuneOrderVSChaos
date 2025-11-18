using System;
using UnityEngine;

internal class ShapeHandler
{
    private Camera _camera;
    private Ray _ray;

    private ShapeView _shape;

    public ShapeHandler(Camera camera, Ray ray)
    {
        if (camera == null)
            throw new InvalidOperationException("camera is null");

        _camera = camera;
        _ray = ray;
    }

    internal void RaiseShape()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out RaycastHit hit, _camera.transform.position.y) && hit.transform.TryGetComponent(out ShapeView shape) && shape.IsRaised == false)
        {
            _shape = shape;
            _shape.Raise();
        }
    }

    internal void PutShape()
    {
        if (_shape != null)
            _shape.Put();
    }
}