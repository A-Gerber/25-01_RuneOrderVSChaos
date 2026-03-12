using System;
using UnityEngine;

public class ShapeMover
{
    private readonly float _height;
    private readonly float _speed;
    private readonly Vector2 _minPointBorder;
    private readonly Vector2 _maxPointBorder;
    private Vector3 _offset;

    public ShapeMover(float speed, float height)
    {
        if (speed < 0)
            throw new ArgumentOutOfRangeException(nameof(speed));

        _speed = speed;
        _height = height;
        _minPointBorder = Constants.MinLimitsForLeavingArena;
        _maxPointBorder = Constants.MaxLimitsForLeavingArena;
    }

    internal void CalculateOffset(Transform transform)
    {
        _offset = transform.position - UserUtilities.GetCursorPosition(_height);
        _offset.y = 0f;
    }

    internal void Move(Transform transform, float verticalShift)
    {
        Vector3 targetPosition = UserUtilities.GetCursorPosition(_height) + _offset + Vector3.forward * verticalShift;

        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, _minPointBorder.x, _maxPointBorder.x),
            targetPosition.y,
            Mathf.Clamp(targetPosition.z, _minPointBorder.y, _maxPointBorder.y));

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
    }
}