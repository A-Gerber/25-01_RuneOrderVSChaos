using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Score—ounterPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textCombo;
    [SerializeField] private TextMeshProUGUI _textIncreasedCombo;
    [SerializeField] private TextMeshProUGUI _textGood;
    [SerializeField] private TextMeshProUGUI _textExcellent;
    [SerializeField] private float _delayInShowScored = 0.5f;

    private Score—ounter _Òounter;
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

    public void Initialize(Score—ounter Òounter)
    {
        if (_Òounter != null)
        {
            _Òounter.ShowedScore -= OnShow;
            _Òounter.UpdatedTimeFrame -= OnUpdateTimeFrame;
        }

        _Òounter = Òounter ?? throw new InvalidOperationException("Òounter is null");

        _Òounter.ShowedScore += OnShow;
        _Òounter.UpdatedTimeFrame += OnUpdateTimeFrame;
    }

    private void OnUpdateTimeFrame()
    {
        _waitTimeFrame = new WaitForSeconds(_Òounter.TimeFrameOfCombo);
    }

    private void OnShow(int numberOfCombos)
    {
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

        if (_Òounter.TotalComboNumber <= _Òounter.NumberSimpleCombo)
        {
            text = _textGood;
        }
        else if (_Òounter.TotalComboNumber >= _Òounter.ComboSkillPointsInterval)
        {
            if (_Òounter.TotalComboNumber % _Òounter.ComboSkillPointsInterval == 0)
            {
                text = _textExcellent;
            }
            else
            {
                text = _textIncreasedCombo;
                text.text = $"COMBO {_Òounter.TotalComboNumber}!";
            }
        }
        else
        {
            text = _textCombo;
            text.text = $"Combo {_Òounter.TotalComboNumber}!";
        }

        text.rectTransform.position = Input.mousePosition;
        text.alpha = 1f;
        StartCoroutine(DisableTextOverTime(text));
    }

    private IEnumerator DisableTextOverTime(TextMeshProUGUI text)
    {
        yield return _waitDelay;
        text.alpha = 0f;
    }

    private IEnumerator ResetCounterOverTime()
    {
        yield return _waitTimeFrame;
        _Òounter.ResetCounter();
        _isOnCountdown = false;
    }
}