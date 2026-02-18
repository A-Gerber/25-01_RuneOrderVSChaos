using System;
using System.Collections;
using TMPro;
using UnityEngine;

internal class EnemySkillPerfomerView : MonoBehaviour
{
    [SerializeField] private float _delay = 1f;
    [SerializeField] private TextMeshProUGUI _healingValueText;

    private EnemySkillPerfomer _skillPerfomer;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
    }

    internal void Initialize(EnemySkillPerfomer skillPerfomer)
    {
        if (_skillPerfomer != null)
        {
            _skillPerfomer.UsedHealingSkill -= OnUseHealingSkill;
            _skillPerfomer.UsedFreezingSkill -= OnUseFreezingSkill;
            _skillPerfomer.UsedGroundImpact -= OnUseGroundImpact;
        }

        _skillPerfomer = skillPerfomer ?? throw new InvalidOperationException("skillPerfomer is null");

        _skillPerfomer.UsedHealingSkill += OnUseHealingSkill;
        _skillPerfomer.UsedFreezingSkill += OnUseFreezingSkill;
        _skillPerfomer.UsedGroundImpact += OnUseGroundImpact;
    }

    private void OnUseGroundImpact(Vector3 position)
    {

    }

    private void OnUseFreezingSkill(Vector3 position)
    {

    }

    private void OnUseHealingSkill(int vallue)
    {
        _healingValueText.gameObject.SetActive(true);
        _healingValueText.text = $"+{vallue}";

        StartCoroutine(DisableGameObjectOverTime(_healingValueText.gameObject));
    }

    private IEnumerator DisableGameObjectOverTime(GameObject @object)
    {
        yield return _wait;
        @object.SetActive(false);
    }
}
