using UnityEngine;

internal class ShapeModelFactory
{
    private readonly float _height;

    public ShapeModelFactory()
    {
        _height = Constants.CameraHeight - Constants.FlightAltitude;
    }

    internal Shape Create(Transform transform, float speed)
    {
        ShapeMover mover = new (speed, _height);
        return new Shape(transform, mover);
    }
}