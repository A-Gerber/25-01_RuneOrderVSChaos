using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapeView : MonoBehaviour
{
    [SerializeField] private Transform _cubeContainer;
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _durationOfReturn = 1f;
    [SerializeField] private float _durationOfReduction = 0.5f;
    [SerializeField] private float _durationOfMagnification = 0.25f;
    [SerializeField] private float _reduceCoefficient = 0.5f;

    private ShapeModel _shapeModel;
    private ShapeMover _mover; //Убрать при создании composite root

    private readonly float _unitCoefficient = 1f;
    private bool _isReduced;

    public event Action<ShapeView> Released;

    public float DurationOfReturn => _durationOfReturn;
    public bool IsRestart { get; private set; } = false;
    public bool IsRaised => _shapeModel.IsRaised;

    private void Awake()
    {
        _mover = new ShapeMover(_speed);
    }

    private void Update()
    {
        if (IsRaised)
            _shapeModel.Raise();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ReducingZone>(out _))
        {
            if (_isReduced == false)
            {
                _cubeContainer.DOScale(_reduceCoefficient, _durationOfReduction).SetEase(Ease.Linear);
                _isReduced = true;
            }

            _shapeModel.SetStatusOnStartPoint();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isReduced && other.TryGetComponent<ReducingZone>(out _))
        {
            _cubeContainer.DOScale(_unitCoefficient, _durationOfMagnification).SetEase(Ease.Linear);
            _isReduced = false;
        }
    }

    public void Initialize(ShapeModel shape, float height)
    {
        if (_shapeModel != null)
            _shapeModel.ReleasedOnRestart -= OnRelease;

        if (height < 0)
            throw new ArgumentOutOfRangeException(nameof(height));

        _shapeModel = shape ?? throw new InvalidOperationException("shape is null");
        _mover.SetHeight(height);

        _shapeModel.ReleasedOnRestart += OnRelease;
    }

    public void Reduce()
    {
        _isReduced = true;
        _cubeContainer.localScale = Vector3.one * _reduceCoefficient;
    }

    public void SetPosition(Vector3 startPosition)
    {
        _shapeModel.SetPosition(startPosition);
    }

    public ShapeMover GetMover()  //Убрать при создании composite root
    {
        return _mover;
    }

    public void TakeCubes(List<CubeView> cubeViews)
    {
        List<CubeModel> cubeModels = new List<CubeModel>();

        foreach (var cubeView in cubeViews)
        {
            cubeView.transform.parent = _cubeContainer;
            cubeView.transform.localPosition = cubeView.LocalPosition;
            cubeModels.Add(cubeView.GetCubeModel());
        }

        _shapeModel.TakeCubes(cubeModels);
    }

    public void RemoveCubes()
    {
        _shapeModel.RemoveCubes();
    }

    public ShapeModel GetShapeModel()
    {
        return _shapeModel;
    }

    public void Raise()
    {
        _shapeModel.SetStatusRaised();
    }

    public void Put()
    {
        _shapeModel.Put();
    }

    private void OnRelease(bool value)
    {
        IsRestart = value;
        _isReduced = false;
        _cubeContainer.localScale = Vector3.one;

        Released?.Invoke(this);
    }
}