using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class AttackerView : MonoBehaviour
{
    [SerializeField] private RectTransform _texts;
    [SerializeField] private TextMeshProUGUI _textScore;
    [SerializeField] private float _delayInShowScore = 1f;


    [Header("Shake")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _perlinNoiseTimeScale = 1f;
    [SerializeField] private AnimationCurve _perlinNoiseAmplitudeCurve;
    [SerializeField] private float _amplitude = 5f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _shakeMultiplier = 2f;

    private AttackerModel _attackerModel;
    private CameraShaker _cameraShaker;
    private WaitForSeconds _waitingForShowScore;

    private void Awake()
    {
        _textScore.alpha = 0f;
        _waitingForShowScore = new WaitForSeconds(_delayInShowScore);

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
        _texts.position = UserUtilities.GetRandomScreenPosition();
        StartCoroutine(ShowScoredOverTime(score));
    }

    private IEnumerator ShowScoredOverTime(int score)
    {
        yield return _waitingForShowScore;
        _textScore.text = $"-{score}";
        _textScore.alpha = 1f;
        StartCoroutine(DisableTextOverTime());
    }

    private IEnumerator DisableTextOverTime()
    {
        yield return _waitingForShowScore;
        _textScore.alpha = 0f;
    }


}