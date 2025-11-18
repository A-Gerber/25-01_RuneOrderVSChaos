using System;
using UnityEngine;

internal class SkillUserView : MonoBehaviour
{
    [SerializeField] private Transform _attackZone;
    [SerializeField] private float _speed = 25f;

    private SkillUser _skillUser;
    private Transform _transform;
    private bool _isEnableAttackZone = false;

    private void Awake()
    {
        _attackZone.gameObject.SetActive(false);
        _transform = transform;
    }

    private void Update()
    {
        if (_isEnableAttackZone)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, GetCursorPosition(), _speed * Time.deltaTime);
        }
    }

    internal void Initialize(SkillUser skillUser)
    {
        if (_skillUser != null)
        {
            _skillUser.EnabledAttackZone -= OnEnableAttackZone;
            _skillUser.DisabledAttackZone -= OnDisableAttackZone;
        }

        _skillUser = skillUser ?? throw new InvalidOperationException("skillUser is null");

        _skillUser.EnabledAttackZone += OnEnableAttackZone;
        _skillUser.DisabledAttackZone += OnDisableAttackZone;
    }

    private Vector3 GetCursorPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _skillUser.Height;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void OnEnableAttackZone()
    {
        _isEnableAttackZone = true;
        _attackZone.gameObject.SetActive(true);
        _transform.position = GetCursorPosition();
    }

    private void OnDisableAttackZone()
    {
        _isEnableAttackZone = false;
        _attackZone.gameObject.SetActive(false);
    }
}