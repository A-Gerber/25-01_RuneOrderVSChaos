using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapePresenter : MonoBehaviour, ILiftable
{
    [SerializeField] private Transform _cubeContainer;
    [SerializeField] private float _durationOfReturn = 1f;
    [SerializeField] private float _durationOfReduction = 0.5f;
    [SerializeField] private float _durationOfMagnification = 0.25f;
    [SerializeField] private float _reduceCoefficient = 0.5f;

    private Shape _shape;
    private Arrow _arrow;
    private ShapeRotater _shapeRotater;
    private MoverTo _moverTo;
    private IChangeableRuneDisplay _area;

    private readonly float _unitCoefficient = 1f;
    private bool _isReduced;
    private bool _canArrowTrackMovements;

    public event Action<ShapePresenter> Released;

    public int CubeCount => _shape.CubeCount;
    public bool IsRaised => _shape.IsRaised;
    public bool IsRestart { get; private set; } = false;

    private void Awake()
    {
        _moverTo = new MoverTo(transform);
    }

    private void FixedUpdate()
    {
        if (_shapeRotater != null && _shape.IsRaised)
            _shapeRotater.Rotate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ReducingZone>(out _))
        {
            if (_isReduced == false)
            {
                _cubeContainer.DOScale(_reduceCoefficient, _durationOfReduction).SetEase(Ease.Linear);
                _isReduced = true;
                _shape.ChangeEffectOnCubes(false);
            }

            _shape.SetStatusOnStartPoint();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isReduced && other.TryGetComponent<ReducingZone>(out _))
        {
            _cubeContainer.DOScale(_unitCoefficient, _durationOfMagnification).SetEase(Ease.Linear);
            _isReduced = false;
            _shape.ChangeEffectOnCubes(true);
        }
    }

    private void Update()
    {
        if (_shape.IsRaised && _shape.IsBackStartPosition == false)
            _shape.Move();

        if (_canArrowTrackMovements)
            _arrow.TrackMovement();

        _moverTo.Move();
    }

    public void Initialize(Shape shape, ShapeRotater shapeRotater, IChangeableRuneDisplay area)
    {
        if (_shape != null)
        {
            _shape.ReleasedOnRestart -= OnRelease;
            _shape.ReturnedOnStartPosition -= OnReturnOnStartPosition;
        }

        _shape = shape ?? throw new InvalidOperationException("shape is null");
        _shapeRotater = shapeRotater ?? throw new InvalidOperationException("shapeRotater is null");
        _area = area ?? throw new InvalidOperationException("area is null");

        _shape.ReleasedOnRestart += OnRelease;
        _shape.ReturnedOnStartPosition += OnReturnOnStartPosition;
    }

    public void Reduce()
    {
        _isReduced = true;
        _cubeContainer.localScale = Vector3.one * _reduceCoefficient;
        _shape.ChangeEffectOnCubes(false);
    }

    public void SetPosition(Vector3 startPosition)
    {
        _shape.SetPosition(startPosition);
    }

    public void Take(List<CubeView> cubeViews, Arrow arrow)
    {
        if (cubeViews == null)
            throw new InvalidOperationException("cubeViews is null");

        if (cubeViews.Count == 0)
            throw new InvalidOperationException("cubeViews is empty");

        List<Cube> cubeModels = new();

        foreach (var cubeView in cubeViews)
        {
            if (cubeViews != null)
            {
                cubeView.transform.parent = _cubeContainer;
                cubeView.transform.localPosition = cubeView.LocalPosition;
                cubeModels.Add(cubeView.GetCubeModel());
            }
        }

        _shape.TakeCubes(cubeModels);
        _arrow = arrow ?? throw new InvalidOperationException("arrow is null");
    }

    public void RemoveCubes()
    {
        _shape.RemoveCubes();
    }

    public Shape GetShapeModel()
    {
        return _shape;
    }

    private void OnReturnOnStartPosition(Vector3 target)
    {
        _moverTo.SetTarget(target, _durationOfReturn);
    }

    private void OnRelease(bool value)
    {
        IsRestart = value;
        _isReduced = false;
        _cubeContainer.localScale = Vector3.one;

        _arrow.Activate();
        _arrow.Destroy();
        _moverTo.Reset();
        Released?.Invoke(this);
    }

    public void SetStatusRaised(Vector3 cubePosition)
    {
        _area.ChangeState(true);
        _shape.SetStatusRaised(cubePosition);
        _canArrowTrackMovements = true;
    }

    public void Put()
    {
        _canArrowTrackMovements = false;

        if (_shape.TryPut() == false)
            _arrow.Clear();

        _area.ChangeRuneDisplay();
        _area.ChangeState(false);
    }
}