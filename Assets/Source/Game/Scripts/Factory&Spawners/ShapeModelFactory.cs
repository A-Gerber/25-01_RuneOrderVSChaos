using UnityEngine;

internal class ShapeModelFactory
{
    internal ShapeModel Create(Transform transform, ShapeMover shapeMover, float durationOfReturn)
    {
        return new ShapeModel(shapeMover, transform, durationOfReturn);
    }
}