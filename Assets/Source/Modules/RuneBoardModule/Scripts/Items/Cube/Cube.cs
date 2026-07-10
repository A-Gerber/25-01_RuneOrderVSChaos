using System;
using UnityEngine;

public class Cube : IReleasable
{
    private readonly Transform _transform;
    private readonly Rigidbody _rigidbody;
    private readonly FrozenState _defaultFrozenState = new(false);
    private readonly float _distanceRaycast;
    private CellPresenter _cellView;

    public Cube(Transform transform, Rigidbody rigidbody, float distanceRayCast)
    {
        if (distanceRayCast <= 0)
            throw new ArgumentOutOfRangeException(nameof(distanceRayCast));

        _transform = transform != null ? transform : throw new InvalidOperationException("transform is null");
        _rigidbody = rigidbody != null ? rigidbody : throw new InvalidOperationException("rigidbody is null");
        _distanceRaycast = distanceRayCast;
    }

    internal LocalPosition LocalPosition { get; private set; }
    internal Vector3 Position => _transform.position;
    internal bool IsFrozen { get; private set; } = false;

    internal event Action Released;
    internal event Action Pushed;
    internal event Action<CubeState> ChangingState;
    internal event Action<Vector3> Landed;

    public bool TryRelease()
    {
        if (IsFrozen)
        {
            ChangeState(_defaultFrozenState);
            return false;
        }

        Released?.Invoke();
        return true;
    }

    public void Restart()
    {
        ChangeState(_defaultFrozenState);
        Released?.Invoke();
    }

    public void PushAtPoint(Vector3 targetPosition, float force)
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForceAtPosition(Vector3.up * force, targetPosition, ForceMode.Impulse);
        Pushed?.Invoke();
    }

    internal void Land()
    {
        _transform.SetParent(_cellView.transform);
        _cellView.Take(this);
        Landed?.Invoke(_cellView.transform.position);
    }

    internal bool TryGetBusyCell()
    {
        if (Physics.Raycast(_transform.position, Vector3.down, out RaycastHit hit, _distanceRaycast))
        {
            if (hit.transform.TryGetComponent(out CellPresenter target) && target.IsBusy == false)
            {
                _cellView = target;
                return false;
            }
        }

        return true;
    }

    internal void ChangeState(CubeState state)
    {
        if (state is FrozenState frozenState)
            IsFrozen = frozenState.Value;

        ChangingState?.Invoke(state);
    }

    internal void SetLocalPosition(LocalPosition localPosition)
    {
        LocalPosition = localPosition;
    }
}