using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class EnemySkillPerfomerPresenter : MonoBehaviour
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

    private void FixedUpdate()
    {       
        if (_skillPerfomer == null || !_skillPerfomer.CanUseSkill)
            return;

        _currentTime -= Time.fixedDeltaTime;
        _circularSlider.fillAmount = _currentTime / _skillPerfomer.EnemySkillCooldown;

        if (_currentTime <= 0f)
            _skillPerfomer.UseSkill();
    }

    private void OnDestroy()
    {
        _skillPerfomer?.Unsubscribe();
    }

    internal void Initialize(EnemySkillPerfomer skillPerfomer)
    {
        if (_skillPerfomer != null)
        {
            _skillPerfomer.UsedHealingSkill -= OnUseHealingSkill;
            _skillPerfomer.UsedSkill -= OnUseSkill;
            _skillPerfomer.Initialized -= OnResetTimer;
        }

        _skillPerfomer = skillPerfomer ?? throw new InvalidOperationException("skillPerfomer is null");

        if (_skillPerfomer != null)
        {
            _skillPerfomer.UsedHealingSkill += OnUseHealingSkill;
            _skillPerfomer.UsedSkill += OnUseSkill;
            _skillPerfomer.Initialized += OnResetTimer;
        }
    }

    private void OnUseSkill()
    {
        if (enabled)
            _currentTime = _skillPerfomer.EnemySkillCooldown;
    }

    private void OnUseHealingSkill(int vallue)
    {
        if (enabled == false)
            return;

        _currentTime = _skillPerfomer.EnemySkillCooldown;
        _healingValueText.gameObject.SetActive(true);
        _healingValueText.text = $"+{vallue}";

        StartCoroutine(DisableGameObjectOverTime(_healingValueText.gameObject));
    }

    private void OnResetTimer()
    {
        if (enabled)
            _currentTime = _skillPerfomer.EnemySkillCooldown;
    }

    private IEnumerator DisableGameObjectOverTime(GameObject @object)
    {
        yield return _wait;
        @object.SetActive(false);
    }
}