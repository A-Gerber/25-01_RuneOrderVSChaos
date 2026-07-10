using System;
using System.Collections;
using TMPro;
using UnityEngine;

internal class ScoreCounterPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textCombo;
    [SerializeField] private TextMeshProUGUI _textIncreasedCombo;
    [SerializeField] private TextMeshProUGUI _textGood;
    [SerializeField] private TextMeshProUGUI _textExcellent;
    [SerializeField] private float _delayInShowScored = 1f;

    private ScoreCounter _ñounter;
    private WaitForSeconds _waitTimeFrame;
    private WaitForSeconds _waitDelay;
    private Coroutine _coroutine;

    private bool _isOnCountdown = false;

    private void Awake()
    {
        _textCombo.alpha = 0f;
        _textIncreasedCombo.alpha = 0f;
        _textGood.alpha = 0f;
        _textExcellent.alpha = 0f;
        _waitDelay = new WaitForSeconds(_delayInShowScored);
    }

    public void Initialize(ScoreCounter counter)
    {
        if (_ñounter != null)
        {
            _ñounter.ShowedScore -= OnShow;
            _ñounter.UpdatedTimeFrame -= OnUpdateTimeFrame;
        }

        _ñounter = counter ?? throw new InvalidOperationException("counter is null");
        _waitTimeFrame = new WaitForSeconds(_ñounter.TimeFrameOfCombo);

        if (_ñounter != null)
        {
            _ñounter.ShowedScore += OnShow;
            _ñounter.UpdatedTimeFrame += OnUpdateTimeFrame;
        }
    }

    private void OnUpdateTimeFrame()
    {
        if (enabled)
            _waitTimeFrame = new WaitForSeconds(_ñounter.TimeFrameOfCombo);
    }

    private void OnShow(int numberOfCombos)
    {
        if (enabled == false)
            return;

        ShowComboText();

        if (_isOnCountdown && _coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ResetCounterOverTime());
        _isOnCountdown = true;
    }

    private void ShowComboText()
    {
        TextMeshProUGUI text;

        if (_ñounter.TotalComboNumber <= _ñounter.NumberSimpleCombo)
        {
            text = _textGood;
        }
        else if (_ñounter.TotalComboNumber % _ñounter.ComboSkillPointsInterval == 0)
        {
            text = _textExcellent;
        }
        else if (_ñounter.TotalComboNumber >= _ñounter.ComboSkillPointsInterval && _ñounter.TotalComboNumber % _ñounter.ComboSkillPointsInterval != 0)
        {
            text = _textIncreasedCombo;
            text.text = $"COMBO {_ñounter.TotalComboNumber}!";
        }
        else
        {
            text = _textCombo;
            text.text = $"Combo {_ñounter.TotalComboNumber}!";
        }

        StartCoroutine(DisableTextOverTime(text));
    }

    private IEnumerator DisableTextOverTime(TextMeshProUGUI text)
    {
        text.alpha = 1f;
        yield return _waitDelay;
        text.alpha = 0f;
    }

    private IEnumerator ResetCounterOverTime()
    {
        yield return _waitTimeFrame;
        _ñounter.ResetCounter();
        _isOnCountdown = false;
    }
}