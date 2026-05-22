using UnityEngine;

public class MoverTo
{
    private readonly Transform _transform;
    private Vector3 _target;
    private float _speed = 0f;
    private bool _haveTarget = false;

    public MoverTo(Transform transform)
    {
        _transform = transform;
    }

    public void Move()
    {
        if (!_haveTarget)
            return;

        _transform.position = Vector3.MoveTowards(_transform.position, _target,_speed * Time.deltaTime);

        if ((_transform.position - _target).sqrMagnitude < Constants.CloseDistance)
        {
            _transform.position = _target;
            _haveTarget = false;
        }
    }

    public void Reset()
    {
        _haveTarget = false;
    }

    public void SetTarget(Vector3 target, float duration)
    {
        _target = target;
        _speed = Vector3.Distance(_transform.position, target) / duration;
        _haveTarget = true;
    }
}