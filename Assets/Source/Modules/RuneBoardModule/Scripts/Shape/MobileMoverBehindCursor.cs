using UnityEngine;

internal class MobileMoverBehindCursor : MoverBehindCursor
{
    internal MobileMoverBehindCursor(float gridStep, float speed) : base(gridStep, speed)
    { }

    override internal void CalculateOffset(Vector3 cubePosition)
    { }
}