using System;
using UnityEngine;

public class ArrowView : MonoBehaviour
{
    private Arrow _arrow;
    private Transform _transform;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        _transform = transform;
    }

    public void Initialize(Arrow arrow)
    {
        if (_arrow != null)
        {
            _arrow.Destroyed -= OnDestroy;
        }

        _arrow = arrow ?? throw new InvalidOperationException("arrow is null");

        _arrow.Destroyed += OnDestroy;

        float signedAngle = Vector3.SignedAngle(Vector3.forward, _arrow.Direction, Vector3.up);
        _transform.rotation = Quaternion.Euler(0, signedAngle, 0);
    }

    private void OnDestroy()
    {
        if (_arrow != null)
        {
            _arrow.Destroyed -= OnDestroy;
        }

        Destroy(gameObject);
    }

    public Arrow GetArrow() 
    { 
        return _arrow;
    }
}