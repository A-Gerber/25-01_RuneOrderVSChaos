using DG.Tweening;
using System;
using UnityEngine;

public class MoverTo
{
    private readonly Transform _transform;

    public MoverTo(Transform transform)
    {
        _transform = transform;
    }

    public void MoveTo(Vector3 targetPosition, float duration)
    {
        if (_transform == null)
            throw new InvalidOperationException("transform is null");

        if (targetPosition == null)
            throw new InvalidOperationException("targetPosition is null");

        _transform.DOMove(targetPosition, duration);
    }
}