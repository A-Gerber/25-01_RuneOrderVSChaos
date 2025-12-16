using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPresenter : MonoBehaviour
{
    private const float Delay = 0.01f;

    [SerializeField] private Image _enemyView;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _smoothEffectTime = 0.25f;

    private SimpleEnemyModel _enemyModel;
    private Coroutine _coroutine;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(Delay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WizardProjectile bullet))       
            bullet.Release();       
    }

    public void SetEnemy(SimpleEnemyModel simpleEnemyModel)
    {
        if (_enemyModel != null)
            _enemyModel.ChangedHealth -= Show;

        _enemyModel = simpleEnemyModel ?? throw new InvalidOperationException("simpleEnemyModel is null");

        _enemyModel.ChangedHealth += Show;
        _enemyModel.SetMaxHealth();
        _enemyView.sprite = _enemyModel.Icon;
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