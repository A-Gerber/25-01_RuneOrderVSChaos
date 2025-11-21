using System;
using UnityEngine;

internal class CameraShaker
{
    
    private Transform _cameraTransform;
    private float _perlinNoiseTimeScale = 1f;
    private AnimationCurve _perlinNoiseAmplitudeCurve;
    private Vector3 _startAngles;
    private Vector3 _shakeAngles;

    private float _amplitude = 5f;
    private float _duration = 1f;
    private float _shakeTimer;
    private float _shakeTimerValue = 1f;

    internal CameraShaker(Transform cameraTransform, float perlinNoiseTimeScale, AnimationCurve perlinNoiseAmplitudeCurve)
    {
        _cameraTransform = cameraTransform ?? throw new InvalidOperationException("cameraTransform is null");
        _perlinNoiseAmplitudeCurve = perlinNoiseAmplitudeCurve ?? throw new InvalidOperationException("perlinNoiseAmplitudeCurve is null");
        _perlinNoiseTimeScale = perlinNoiseTimeScale;
        _startAngles = cameraTransform.localRotation.eulerAngles;
        _shakeTimer = -1f;
    }

    internal void MakeShake(float amplitude, float duration)
    {
        _amplitude = amplitude;
        _duration = Mathf.Max(duration, 0.05f);
        _shakeTimer = _shakeTimerValue;
    }

    internal void UpdateShake()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime / _duration;

            float time = Time.time * _perlinNoiseTimeScale;

            _shakeAngles.x = Mathf.PerlinNoise(time, 0);
            _shakeAngles.y = Mathf.PerlinNoise(0, time);
            _shakeAngles.z = Mathf.PerlinNoise(time, time);

            _shakeAngles *= _amplitude;
            _shakeAngles *= _perlinNoiseAmplitudeCurve.Evaluate(Mathf.Clamp01(1 - _shakeTimer));

            _shakeAngles.x += _startAngles.x;
            _cameraTransform.localEulerAngles = _shakeAngles;
        }
    }
}
