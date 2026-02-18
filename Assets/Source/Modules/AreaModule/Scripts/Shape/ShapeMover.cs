using System;
using UnityEngine;

public class ShapeMover
{
    private readonly float _height;
    private readonly float _speed;
    private Vector2 _minPointBorder;
    private Vector2 _maxPointBorder;

    public ShapeMover(float speed, float height)
    {
        if (speed < 0)
            throw new ArgumentOutOfRangeException(nameof(speed));

        _speed = speed;
        _height = height;
        _minPointBorder = new Vector2(-2.5f, -4.5f);
        _maxPointBorder = new Vector2(9.5f, 8.5f);
    }

    internal void Move(Transform transform)
    {
        if (transform == null)
            throw new InvalidOperationException("transform is null");
        
        Vector3 targetPosition = UserUtilities.GetCursorPosition(_height);
        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, _minPointBorder.x, _maxPointBorder.x), 
            targetPosition.y, 
            Mathf.Clamp(targetPosition.z, _minPointBorder.y, _maxPointBorder.y));

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
    }
}