using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ManaGeneratorView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _manaCountText;
    [SerializeField] private float _durationEffectDepletion = 1f;
    [SerializeField] private float _delay = 0.1f;

    private ManaGenerator _manaGenerator;
    private WaitForSeconds _wait;
    private Coroutine _coroutine;
    private float _duration;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
    }

    internal void Initialize(ManaGenerator manaGenerator)
    {
        if (_manaGenerator != null)
        {
            _manaGenerator.ManaCountChanged -= OnChangeManaCount;
            _manaGenerator.ManaDepleted -= OnShowManaDepletion;
        }

        _manaGenerator = manaGenerator ?? throw new InvalidOperationException("manaGenerator is null");

        _manaGenerator.ManaCountChanged += OnChangeManaCount;
        _manaGenerator.ManaDepleted += OnShowManaDepletion;

        _manaCountText.text = $"{_manaGenerator.ManaCount}";
    }

    private void OnShowManaDepletion()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _duration = _durationEffectDepletion;
        _coroutine = StartCoroutine(ShowManaDepletion());
    }

    private void OnChangeManaCount(int count)
    {
        _manaCountText.text = $"{count}";
    }

    private IEnumerator ShowManaDepletion()
    {
        while (_duration > 0f)
        {
            _manaCountText.color = Color.red;
            yield return _wait;
            _manaCountText.color = Color.white;
            yield return _wait;
            _duration -= _delay;
        }
    }
}