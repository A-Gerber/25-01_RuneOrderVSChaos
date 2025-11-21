using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class AttackerPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textScore;
    [SerializeField] private TextMeshProUGUI _textCombo;
    [SerializeField] private TextMeshProUGUI _textIncreasedCombo;
    [SerializeField] private TextMeshProUGUI _textGood;
    [SerializeField] private TextMeshProUGUI _textExcellent;
    [SerializeField] private float _textHeight = 18f;
    [SerializeField] private float _timeframeOfCombo = 3f;
    [SerializeField] private float _delayInShowScored = 0.5f;
    [SerializeField] private int _divider = 5;
    [SerializeField] private int _numberSimpleCombo = 1;

    [Header("Shake")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _perlinNoiseTimeScale = 1f;
    [SerializeField] private AnimationCurve _perlinNoiseAmplitudeCurve;

    [SerializeField] private float _amplitude = 5f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _shakeMultiplier = 2f;

    private AttackerModel _attackerModel;
    private CameraShaker _cameraShaker;
    private int _totalComboNumber = 0;
    private WaitForSeconds _waitTimeframe;
    private WaitForSeconds _waitDelay;
    private Coroutine _coroutine;
    private bool _isOnCountdown = false;

    private void Awake()
    {
        _textScore.alpha = 0f;
        _textCombo.alpha = 0f;
        _textIncreasedCombo.alpha = 0f;
        _textGood.alpha = 0f;
        _textExcellent.alpha = 0f;
        _waitTimeframe = new WaitForSeconds(_timeframeOfCombo);
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
            _attackerModel.FilledInLines -= OnFillInLine;
            _attackerModel.CubesReleased -= OnShowScored;
            _attackerModel.UsedSkill -= OnUseSkill;
        }

        _attackerModel = attackerModel ?? throw new InvalidOperationException("attackerModel is null");

        _attackerModel.FilledInLines += OnFillInLine;
        _attackerModel.CubesReleased += OnShowScored;
        _attackerModel.UsedSkill += OnUseSkill;
    }

    private void OnUseSkill()
    {
        _cameraShaker.MakeShake(_amplitude, _duration * _shakeMultiplier);
    }

    private void OnFillInLine(int numberOfCombos)
    {
        _totalComboNumber += numberOfCombos;

        if (_totalComboNumber > _numberSimpleCombo)
            _cameraShaker.MakeShake(_amplitude, _duration);

        ShowComboText();

        if (_isOnCountdown)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(ResetCounterOverTime());
        _isOnCountdown = true;
    }

    private void ShowComboText()
    {
        TextMeshProUGUI text;

        if (_totalComboNumber == _numberSimpleCombo)
        {
            text = _textGood;
        }
        else if (_totalComboNumber >= _divider)
        {
            _attackerModel.SendNumberOfSkillPoints(_totalComboNumber / _divider);

            if (_totalComboNumber % _divider == 0)
            {
                text = _textExcellent;
            }
            else
            {
                text = _textIncreasedCombo;
                text.text = $"COMBO {_totalComboNumber}!";
            }
        }
        else
        {
            text = _textCombo;
            text.text = $"Combo {_totalComboNumber}!";
        }

        text.rectTransform.position = Input.mousePosition;
        text.alpha = 1f;
        StartCoroutine(DisableTextOverTime(text));
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
        StartCoroutine(DisableTextOverTime(_textScore));
    }

    private IEnumerator DisableTextOverTime(TextMeshProUGUI text)
    {
        yield return _waitDelay;
        text.alpha = 0f;
    }

    private IEnumerator ResetCounterOverTime()
    {
        yield return _waitTimeframe;
        _totalComboNumber = 0;
        _isOnCountdown = false;
    }
}