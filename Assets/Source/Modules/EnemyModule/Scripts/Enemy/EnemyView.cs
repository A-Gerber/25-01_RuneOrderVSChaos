using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    private const float Delay = 0.01f;

    private SimpleEnemyModel _enemyModel;
    private Slider _slider;
    private float _smoothEffectTime;
    private TextMeshProUGUI _text;
    private Coroutine _coroutine;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(Delay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WizardProjectile bullet))
        {
            bullet.Release();
            Show();
        }
    }

    public void Initialize(TextMeshProUGUI text, Slider slider, float smoothEffectTime)
    {
        if (smoothEffectTime <= 0)
            throw new ArgumentOutOfRangeException(nameof(smoothEffectTime));

        _smoothEffectTime = smoothEffectTime;
        _text = text != null ? text : throw new InvalidOperationException("text is null");
        _slider = slider != null ? slider : throw new InvalidOperationException("slider is null");
    }

    public void SetEnemy(SimpleEnemyModel simpleEnemyModel)
    {
        if (_enemyModel != null)
            _enemyModel.ChangedHealth -= Show;

        _enemyModel = simpleEnemyModel ?? throw new InvalidOperationException("simpleEnemyModel is null");
        _enemyModel.SetMaxHealth();
        _enemyModel.ChangedHealth += Show;
        Show();
    }

    private void Show()
    {
        _text.text = $"{_enemyModel.Health} / {_enemyModel.MaxHealth}";
        float sliderValue = (float)_enemyModel.Health / _enemyModel.MaxHealth;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeValueOfSlider(sliderValue));
    }

    private IEnumerator ChangeValueOfSlider(float targetValue)
    {
        float step = Mathf.Abs((targetValue - _slider.value) / _smoothEffectTime);

        while (Mathf.Approximately(_slider.value, targetValue) == false)
        {
            yield return _wait;
            _slider.value = Mathf.MoveTowards(_slider.value, targetValue, step * Time.deltaTime);
        }
    }
}