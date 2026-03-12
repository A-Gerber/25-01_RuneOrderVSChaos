using System;
using UnityEngine;

public class Cube : IReleaseable
{
    private readonly Transform _transform;
    private readonly Rigidbody _rigidbody;
    private readonly float _distanceRaycast;
    private CellView _cellView;
    private bool _isFrozen = false;

    public Cube(Transform transform, Rigidbody rigidbody, float distanceRaycast)
    {
        if (distanceRaycast <= 0)
            throw new ArgumentOutOfRangeException(nameof(distanceRaycast));

        _transform = transform != null ? transform : throw new InvalidOperationException("transform is null");
        _rigidbody = rigidbody != null ? rigidbody : throw new InvalidOperationException("rigidbody is null");
        _distanceRaycast = distanceRaycast;
    }

    internal LocalPosition LocalPosition { get; private set; }
    internal bool IsFrozen => _isFrozen;

    internal event Action Released;
    internal event Action Pushed;
    internal event Action ChangedFreeze;
    internal event Action<bool> ChangedTransparente;
    internal event Action<bool> ChangedGlowEffect;
    internal event Action<Vector3>Landed;

    public void Release()
    {
        if (_isFrozen)
            SetFreeze(false);
        else
            Released?.Invoke();
    }

    public void Restart()
    {
        SetFreeze(false);
        Released?.Invoke();
    }

    public void PushAtPoint(Vector3 targetPosition, float force)
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForceAtPosition(Vector3.up * force, targetPosition, ForceMode.Impulse);
        Pushed?.Invoke();
    }

    internal void TrackLanding()
    {
        if (Physics.Raycast(_transform.position, Vector3.down, out RaycastHit hit, _distanceRaycast) && hit.transform.TryGetComponent(out IDisplayChangeable target))
            target.ChangeDisplayRune();
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
            if (hit.transform.TryGetComponent(out CellView target) && target.IsBusy == false)
            {
                target.DisableRune();
                _cellView = target;

                return false;
            }
        }

        return true;
    }

    internal void Freeze()
    {
        SetFreeze(true);
    }

    internal void SetLocalPosition(LocalPosition localPosition)
    {
        LocalPosition = localPosition;
    }

    internal void ChangeGlowEffect(bool isNormalSize)
    {
        ChangedGlowEffect?.Invoke(isNormalSize);
    }

    internal void SetTransparency(bool value)
    {
        ChangedTransparente?.Invoke(value);
    }

    private void SetFreeze(bool isFrozen)
    {
        _isFrozen = isFrozen;
        ChangedFreeze?.Invoke();
    }

}