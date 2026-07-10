using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScoreCounterPresenter))]
internal class AttackerView : MonoBehaviour
{
    [SerializeField] private Button _openScreenButton;
    [SerializeField] private RectTransform _texts;
    [SerializeField] private TextMeshProUGUI _textScore;
    [SerializeField] private TextMeshProUGUI _textMultiplier;
    [SerializeField] private float _delayInShowScore = 1f;

    private WaitForSeconds _waitingForShowScore;

    internal event Action OpenButtonClicked;

    private void Awake()
    {
        _textScore.alpha = 0f;
        _waitingForShowScore = new WaitForSeconds(_delayInShowScore);
    }

    private void OnEnable()
    {
        _openScreenButton.onClick.AddListener(() => OpenButtonClicked?.Invoke());
    }

    private void OnDisable()
    {
        _openScreenButton.onClick.RemoveListener(() => OpenButtonClicked?.Invoke());
    }

    internal void OnShowScored(int score)
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

    internal void OnShowMultiplier(int count)
    {
        _textMultiplier.text = $"x{count}";
    }
}