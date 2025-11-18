using System;
using UnityEngine;

public class CubeModel
{
    private readonly Transform _transform;
    private readonly float _durationLanding;
    private readonly float _distanceRaycast;
    private readonly MoverTo _moverTo;
    private CellView _cellView;

    public CubeModel(Transform transform, float durationLanding, float distanceRaycast)
    {
        _transform = transform != null ? transform : throw new InvalidOperationException("transform is null");

        if (durationLanding <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationLanding));

        if (distanceRaycast <= 0)
            throw new ArgumentOutOfRangeException(nameof(distanceRaycast));

        _durationLanding = durationLanding;
        _distanceRaycast = distanceRaycast;
        _moverTo = new MoverTo(_transform);
    }

    internal LocalPosition LocalPosition { get; private set; }

    internal event Action Released;

    internal void TrackLanding()
    {
        if (Physics.Raycast(_transform.position, Vector3.down, out RaycastHit hit, _distanceRaycast) && hit.transform.TryGetComponent(out IChangeableColor target))
            target.ChangeColorCells();
    }

    internal void Land()
    {
        _transform.SetParent(_cellView.transform);
        _moverTo.MoveTo(_cellView.transform.position, _durationLanding);
        _cellView.Take(this);
    }

    internal bool TryGetBusyCell()
    {
        if (Physics.Raycast(_transform.position, Vector3.down, out RaycastHit hit, _distanceRaycast))
        {
            if (hit.transform.TryGetComponent(out CellView target) && target.IsBusy == false)
            {
                target.SetDefaultColorCell();
                _cellView = target;

                return false;
            }
        }

        return true;
    }

    internal void Release()
    {
        Released?.Invoke();
    }

    internal void SetLocalPosition(LocalPosition localPosition)
    {
        LocalPosition = localPosition;
    }
}