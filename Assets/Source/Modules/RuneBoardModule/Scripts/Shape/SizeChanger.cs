using DG.Tweening;
using System;
using UnityEngine;

internal class SizeChanger : MonoBehaviour
{
    [SerializeField] private float _durationOfReduction = 0.20f;
    [SerializeField] private float _durationOfMagnification = 0.3f;
    [SerializeField] private float _reduceCoefficient = 0.5f;

    private Transform _cubeContainer;

    internal float DurationOfReduction => _durationOfReduction;

    internal void Set(Transform cubeContainer)
    {
        _cubeContainer = cubeContainer != null ? cubeContainer : throw new InvalidOperationException("cubeContainer is null");
    }

    internal void SmoothChangeSize(bool isReduced)
    {
        if (isReduced)
            _cubeContainer.DOScale(_reduceCoefficient, _durationOfReduction).SetEase(Ease.Linear);
        else
            _cubeContainer.DOScale(Constants.UnitCoefficient, _durationOfMagnification).SetEase(Ease.Linear);
    }

    internal void ChangeSize(bool isReduced)
    {
        if (isReduced)
            _cubeContainer.localScale = Vector3.one * _reduceCoefficient;
        else
            _cubeContainer.localScale = Vector3.one;
    }
}
