using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPresenter : MonoBehaviour, IChangeableLanguage
{
    private const float DelaySlider = 0.01f;

    [SerializeField] private Image _enemyView;
    [SerializeField] private Image _skillIcon;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _smoothEffectTime = 0.25f;

    private TextMeshProUGUI _skillDescription;
    private IEnemy _enemy;
    private Coroutine _coroutine;
    private WaitForSeconds _waitForSlider;

    private void Awake()
    {
        _waitForSlider = new WaitForSeconds(DelaySlider);
    }

    public void ChangeSkillDescription(Languages language)
    {
        if (_enemy != null)
        {
            _enemy.ChangeSkillDescription(language);
            _skillDescription.text = _enemy.SkillDescription;
        }
    }

    internal void Initialize(TextMeshProUGUI skillDescription)
    {
        _skillDescription = skillDescription != null ? skillDescription : throw new ArgumentNullException("skillDescription is null", nameof(skillDescription));
    }

    internal void SetEnemy(IEnemy enemy)
    {
        if (_enemy != null)
            _enemy.ChangedHealth -= Show;

        _enemy = enemy ?? throw new ArgumentNullException("enemy is null", nameof(enemy));

        if (_enemy != null)
            _enemy.ChangedHealth += Show;

        _enemy.UpdateHealth();
        _enemyView.sprite = _enemy.Icon;
        _skillIcon.sprite = _enemy.SkillIcon;
        ChangeSkillDescription(Constants.Language);
    }

    private void Show()
    {
        if (enabled == false)
            return;

        _text.text = $"{_enemy.Health} / {_enemy.MaxHealth}";
        float sliderValue = (float)_enemy.Health / _enemy.MaxHealth;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeValueOfSlider(sliderValue));
    }

    private IEnumerator ChangeValueOfSlider(float targetValue)
    {
        float step = Mathf.Abs((targetValue - _slider.value) / _smoothEffectTime);

        while (Mathf.Approximately(_slider.value, targetValue) == false)
        {
            yield return _waitForSlider;
            _slider.value = Mathf.MoveTowards(_slider.value, targetValue, step * Time.deltaTime);
        }
    }
}