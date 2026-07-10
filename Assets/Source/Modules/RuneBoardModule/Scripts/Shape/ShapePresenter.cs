using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SizeChanger))]
internal class ShapePresenter : MonoBehaviour, ILiftable
{
    [SerializeField] private Transform _cubeContainer;
    [SerializeField] private float _durationOfReturn = 0.5f;

    private Shape _shape;
    private MoverTo _moverTo;
    private SizeChanger _sizeChanger;
    private ShapeRotator _shapeRotator;
    private Arrow _arrow;
    private bool _canArrowTrackMovements = false;

    internal event Action<ShapePresenter> Released;

    public bool IsRaised => _shape.IsRaised;
    internal Transform CubeContainer => _cubeContainer;
    internal int CubeCount => _shape.CubeCount;
    internal bool IsRestart { get; private set; } = false;

    private void Awake()
    {
        _moverTo = new MoverTo(transform);
        _sizeChanger = GetComponent<SizeChanger>();
        _moverTo.MovedToTarget += OnMoveToTarget;
    }

    private void FixedUpdate()
    {
        if (_shapeRotator != null && _shape.IsRaised)
            _shapeRotator.Rotate();
    }

    private void Update()
    {
        _shape.Update();

        if (_canArrowTrackMovements)
            _arrow.TrackMovement();

        _moverTo.Move();
    }

    private void OnDestroy()
    {
        if (_shape != null)
            _moverTo.MovedToTarget -= OnMoveToTarget;
    }

    public void SetStatusRaised(Vector3 cubePosition)
    {
        _shape.SetRaisedState(cubePosition);
        _sizeChanger.SmoothChangeSize(false);
        _canArrowTrackMovements = true;
    }

    public void Land()
    {
        _canArrowTrackMovements = false;

        if (_shape.TryLand() == false)
            _arrow.Clear();
    }

    internal void Initialize(Shape shape, ShapeRotator shapeRotator)
    {
        if (_shape != null)
        {
            _shape.ReleasedOnRestart -= OnRelease;
            _shape.ReturnedOnStartPosition -= OnReturnOnStartPosition;
        }

        _shape = shape ?? throw new ArgumentNullException("shape is null", nameof(shape));
        _shapeRotator = shapeRotator ?? throw new ArgumentNullException("shapeRotator is null", nameof(shapeRotator));

        if (_shape != null)
        {
            _shape.ReleasedOnRestart += OnRelease;
            _shape.ReturnedOnStartPosition += OnReturnOnStartPosition;
        }

        _sizeChanger.Set(_cubeContainer);
    }

    internal Shape GetShapeModel()
    {
        return _shape;
    }

    internal void SetStartParametrs(Vector3 startPosition)
    {
        _shape.SetStartParametrs(startPosition);
        _sizeChanger.ChangeSize(true);
    }

    internal void Take(List<Cube> cubeModels, Arrow arrow)
    {
        _shape.Take(cubeModels);
        _arrow = arrow ?? throw new ArgumentNullException("arrow is null", nameof(arrow));
    }

    private void OnMoveToTarget()
    {
        if (_shape == null)
            return;

        _sizeChanger.SmoothChangeSize(true);
        Invoke(nameof(ChangeToLanded), _sizeChanger.DurationOfReduction);
    }

    private void ChangeToLanded()
    {
        _shape.ChangeToLanded();
    }

    private void OnReturnOnStartPosition(Vector3 target)
    {
        if (enabled)
            _moverTo.SetTarget(target, _durationOfReturn);
    }

    private void OnRelease(bool value)
    {
        if (!enabled)
            return;

        IsRestart = value;
        _sizeChanger.ChangeSize(false);

        _arrow.Activate();
        _arrow.Destroy();
        _moverTo.Reset();
        Released?.Invoke(this);
    }
}