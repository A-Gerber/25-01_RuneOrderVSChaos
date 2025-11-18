using System;
using UnityEngine;

public class WizardProjectile : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _target;
    private bool _canMove;
    private float _speed;

    public event Action<WizardProjectile> Released;

    internal int Damage { get; private set; }

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (_canMove)
        {
            _transform.Translate(_speed * Time.deltaTime * (_target - _transform.position).normalized, Space.World);
        }
    }

    public void Initialize(int damage, float speed, Vector3 target)
    {
        Damage = damage;
        _target = target;
        _speed = speed;
        _canMove = true;
    }

    public void Release()
    {
        _canMove = false;

        Released?.Invoke(this);
    }
}