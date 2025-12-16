using System;
using UnityEngine;

public class UserSkillPerformerView : MonoBehaviour
{
    [SerializeField] private float _speed = 25f;

    private UserSkillPerformer _userSkill;
    private Transform _transform;
    private bool _isEnableAttackZone = false;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (_isEnableAttackZone)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, GetCursorPosition(), _speed * Time.deltaTime);
        }
    }

    public void Initialize(UserSkillPerformer skillUser)
    {
        if (_userSkill != null)
        {
            _userSkill.EnabledAttackZone -= OnEnableAttackZone;
            _userSkill.DisabledAttackZone -= OnDisableAttackZone;
        }

        _userSkill = skillUser ?? throw new InvalidOperationException("skillUser is null");

        _userSkill.EnabledAttackZone += OnEnableAttackZone;
        _userSkill.DisabledAttackZone += OnDisableAttackZone;
    }

    private Vector3 GetCursorPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _userSkill.Height;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void OnEnableAttackZone(ParticleSystem attackZona)
    {
        _isEnableAttackZone = true;
        attackZona.gameObject.SetActive(true);
        _transform.position = GetCursorPosition();
    }

    private void OnDisableAttackZone(ParticleSystem attackZona)
    {
        _isEnableAttackZone = false;
        attackZona.gameObject.SetActive(false);
    }
}