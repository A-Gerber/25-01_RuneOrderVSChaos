using System;
using UnityEngine;

internal class ArrowPresenter : MonoBehaviour
{
    private Arrow _arrow;
    private Transform _transform;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        _transform = transform;
    }

    internal Arrow GetArrow()
    {
        return _arrow;
    }

    internal void Initialize(Arrow arrow)
    {
        if (_arrow != null)
            _arrow.Destroyed -= OnErase;

        _arrow = arrow ?? throw new ArgumentNullException("arrow is null", nameof(arrow));

        if (_arrow != null)
            _arrow.Destroyed += OnErase;

        float signedAngle = Vector3.SignedAngle(Vector3.forward, _arrow.Direction, Vector3.up);
        _transform.rotation = Quaternion.Euler(0, signedAngle, 0);
    }

    private void OnErase()
    {
        if (_arrow != null)
            _arrow.Destroyed -= OnErase;

        Destroy(gameObject);
    }
}