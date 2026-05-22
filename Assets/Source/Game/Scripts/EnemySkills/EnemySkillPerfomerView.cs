using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

internal class EnemySkillPerfomerView : MonoBehaviour
{
    [SerializeField] private float _delay = 1f;
    [SerializeField] private TextMeshProUGUI _healingValueText;
    [SerializeField] private Image _circularSlider;

    private EnemySkillPerfomer _skillPerfomer;
    private WaitForSeconds _wait;
    private float _currentTime;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void FixedUpdate()
    {
        UseTimerSkill();
    }

    internal void Initialize(EnemySkillPerfomer skillPerfomer)
    {
        Unsubscribe();

        _skillPerfomer = skillPerfomer ?? throw new InvalidOperationException("skillPerfomer is null");

        Subscribe();
    }

    private void UseTimerSkill()
    {
        if (!_skillPerfomer.CanUseSkill)
            return;

        _currentTime -= Time.fixedDeltaTime;
        _circularSlider.fillAmount = _currentTime / _skillPerfomer.EnemySkillCooldown;

        if (_currentTime <= 0f)
        {
            _skillPerfomer.UseSkill();
        }
    }

    private void OnUseGroundImpact(Vector3 position)
    {
        _currentTime = _skillPerfomer.EnemySkillCooldown;
    }

    private void OnUseFreezingSkill(Vector3 position)
    {
        _currentTime = _skillPerfomer.EnemySkillCooldown;
    }

    private void OnUseHealingSkill(int vallue)
    {
        _currentTime = _skillPerfomer.EnemySkillCooldown;
        _healingValueText.gameObject.SetActive(true);
        _healingValueText.text = $"+{vallue}";

        StartCoroutine(DisableGameObjectOverTime(_healingValueText.gameObject));
    }

    private void OnResetTimer()
    {
        _currentTime = _skillPerfomer.EnemySkillCooldown;
    }

    private IEnumerator DisableGameObjectOverTime(GameObject @object)
    {
        yield return _wait;
        @object.SetActive(false);
    }

    private void Subscribe()
    {
        if (_skillPerfomer != null)
        {
            _skillPerfomer.UsedHealingSkill += OnUseHealingSkill;
            _skillPerfomer.UsedFreezingSkill += OnUseFreezingSkill;
            _skillPerfomer.UsedGroundImpact += OnUseGroundImpact;
            _skillPerfomer.Initialized += OnResetTimer;
        }
    }

    private void Unsubscribe()
    {
        if (_skillPerfomer != null)
        {
            _skillPerfomer.UsedHealingSkill -= OnUseHealingSkill;
            _skillPerfomer.UsedFreezingSkill -= OnUseFreezingSkill;
            _skillPerfomer.UsedGroundImpact -= OnUseGroundImpact;
            _skillPerfomer.Initialized -= OnResetTimer;
        }
    }
}
