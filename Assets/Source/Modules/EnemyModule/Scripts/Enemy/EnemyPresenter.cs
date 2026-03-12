using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPresenter : MonoBehaviour
{
    private const float DelaySlider = 0.01f;

    [SerializeField] private Image _enemyView;
    [SerializeField] private Image _skillIcon;
    [SerializeField] private Image _circularSlider;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _skillDescription;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _smoothEffectTime = 0.25f;

    private IEnemy _enemy;
    private IPerformableAttack _game;
    private Coroutine _coroutine;
    private WaitForSeconds _waitForSlider;
    private float _currentTime;

    private void Awake()
    {
        _waitForSlider = new WaitForSeconds(DelaySlider);
    }

    private void FixedUpdate()
    {
        UseTimerSkill();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WizardProjectile bullet))
        {
            bullet.Release();

            if (_game.CanAttack)
                _game.Attack();
        }
    }

    public void SetGame(IPerformableAttack game)
    {
        _game = game ?? throw new InvalidOperationException("game is null");
    }

    public void SetEnemy(IEnemy enemy)
    {
        if (_enemy != null)
            _enemy.ChangedHealth -= Show;

        _enemy = enemy ?? throw new InvalidOperationException("enemy is null");
        _currentTime = _enemy.SkillCooldown;

        _enemy.ChangedHealth += Show;
        _enemy.UpdateHealth();
        _enemyView.sprite = _enemy.Icon;
        _skillIcon.sprite = _enemy.SkillIcon;
        _circularSlider.fillAmount = _currentTime / _enemy.SkillCooldown;
        _skillDescription.text = _enemy.SkillDescription;
    }

    private void UseTimerSkill()
    {
        if (_enemy == null)
            return;

        _currentTime -= Time.fixedDeltaTime;
        _circularSlider.fillAmount = _currentTime / _enemy.SkillCooldown;

        if (_currentTime <= 0f && _enemy.IsAlive)
        {
            _enemy.UseSkill();
            _currentTime = _enemy.SkillCooldown;
        }
    }

    private void Show()
    {
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