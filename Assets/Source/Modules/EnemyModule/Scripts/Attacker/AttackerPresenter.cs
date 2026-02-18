using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class AttackerPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textScore;
    [SerializeField] private float _delayInShowScored = 0.5f;

    [Header("Shake")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _perlinNoiseTimeScale = 1f;
    [SerializeField] private AnimationCurve _perlinNoiseAmplitudeCurve;
    [SerializeField] private float _amplitude = 5f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _shakeMultiplier = 2f;

    private AttackerModel _attackerModel;
    private CameraShaker _cameraShaker;
    private WaitForSeconds _waitDelay;

    private void Awake()
    {
        _textScore.alpha = 0f;
        _waitDelay = new WaitForSeconds(_delayInShowScored);
        _cameraShaker = new CameraShaker(_cameraTransform, _perlinNoiseTimeScale, _perlinNoiseAmplitudeCurve);
    }

    private void Update()
    {
        _cameraShaker.UpdateShake();
    }

    public void Initialize(AttackerModel attackerModel)
    {
        if (_attackerModel != null)
        {
            _attackerModel.CubesReleased -= OnShowScored;
            _attackerModel.UsedSkill -= OnUseSkill;
            _attackerModel.ShakedCamera -= OnShakedCamera;
        }

        _attackerModel = attackerModel ?? throw new InvalidOperationException("attackerModel is null");

        _attackerModel.CubesReleased += OnShowScored;
        _attackerModel.UsedSkill += OnUseSkill;
        _attackerModel.ShakedCamera += OnShakedCamera;
    }

    private void OnShakedCamera()
    {
        _cameraShaker.MakeShake(_amplitude, _duration);
    }

    private void OnUseSkill()
    {
        _cameraShaker.MakeShake(_amplitude, _duration * _shakeMultiplier);
    }

    private void OnShowScored(int score)
    {
        Vector3 position = Input.mousePosition;
        StartCoroutine(ShowScoredOverTime(score, position));
    }

    private IEnumerator ShowScoredOverTime(int score, Vector3 position)
    {
        yield return _waitDelay;
        _textScore.rectTransform.position = position;
        _textScore.text = $"-{score}";
        _textScore.alpha = 1f;
        StartCoroutine(DisableTextOverTime());
    }

    private IEnumerator DisableTextOverTime()
    {
        yield return _waitDelay;
        _textScore.alpha = 0f;
    }
}