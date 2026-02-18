using UnityEngine;

internal class ShapeModelFactory
{
    private readonly float _height;

    public ShapeModelFactory(float height)
    {
        _height = height;
    }

    internal Shape Create(Transform transform, float durationOfReturn, float speed)
    {
        ShapeMover mover = new (speed, _height);

        return new Shape(transform, mover, durationOfReturn);
    }
}