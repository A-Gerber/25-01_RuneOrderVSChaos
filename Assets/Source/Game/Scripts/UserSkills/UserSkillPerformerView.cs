using System;
using UnityEngine;

public class UserSkillPerformerView : MonoBehaviour
{
    [SerializeField] private float _speed = 25f;
    [SerializeField] private ParticleSystem _attackZone;

    private UserSkillPerformer _userSkill;
    private Transform _transformAttackZone;
    private bool _isEnableAttackZone = false;

    private void Awake()
    {
        _transformAttackZone = _attackZone.transform;
        _attackZone.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isEnableAttackZone)
        {
            Vector3 targetPosition = UserUtilities.GetCursorPosition(Constants.CameraHeight);
            targetPosition.y = Constants.CellSize;
            _transformAttackZone.position = Vector3.MoveTowards(_transformAttackZone.position, targetPosition, _speed * Time.deltaTime);      
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

    private void OnEnableAttackZone()
    {
        _transformAttackZone.position = UserUtilities.GetCursorPosition(Constants.CameraHeight);
        _isEnableAttackZone = true;
        _attackZone.gameObject.SetActive(true);
    }

    private void OnDisableAttackZone()
    {
        _isEnableAttackZone = false;
        _attackZone.gameObject.SetActive(false);
    }
}