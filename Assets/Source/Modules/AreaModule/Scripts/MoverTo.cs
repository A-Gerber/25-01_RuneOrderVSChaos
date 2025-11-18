using DG.Tweening;
using System;
using UnityEngine;

internal class MoverTo
{
    private Transform _transform;

    internal MoverTo(Transform transform)
    {
        _transform = transform;
    }

    internal void MoveTo(Vector3 targetPosition, float duration)
    {
        if (_transform == null)
            throw new InvalidOperationException("transform is null");

        if (targetPosition == null)
            throw new InvalidOperationException("targetPosition is null");

        _transform.DOMove(targetPosition, duration);
    }
}