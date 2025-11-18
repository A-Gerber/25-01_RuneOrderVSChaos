using System;
using System.Collections.Generic;
using UnityEngine;

internal class SkillUser
{
    private Skill _skill;
    private float _minBorderArea;
    private float _maxBorderArea;
    private float _height;
    private bool _isPressedButton = false;

    internal SkillUser(Skill skill, float minBorderArea, float maxBorderArea, float height)
    {
        _skill = skill ?? throw new InvalidOperationException("skill is null");

        _minBorderArea = minBorderArea;
        _maxBorderArea = maxBorderArea;
        _height = height;
    }

    internal event Action EnabledAttackZone;
    internal event Action DisabledAttackZone;

    internal float Height => _height;

    internal bool TryGetSkillCoordinates(out List<LocalPosition> coordinates, int skillCount)
    {
        coordinates = new List<LocalPosition>();

        if (_isPressedButton == false || skillCount <= 0)
        {
            DisabledAttackZone?.Invoke();
            _isPressedButton = false;

            return false;
        }

        _isPressedButton = false;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _height;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (IsInRange(targetPosition.x, _minBorderArea, _maxBorderArea) == false || IsInRange(targetPosition.z, _minBorderArea, _maxBorderArea) == false)
        {
            DisabledAttackZone?.Invoke();
            return false;
        }

        coordinates = GetSkillCoordinates(targetPosition);
        return true;
    }

    internal void UseSkill()
    {
        DisabledAttackZone?.Invoke();
        _skill.Use();
    }

    internal void PressButton()
    {
        _isPressedButton = true;
        EnabledAttackZone?.Invoke();
    }

    private List<LocalPosition> GetSkillCoordinates(Vector3 targetPosition)
    {
        LocalPosition position = new LocalPosition((int)Mathf.Round(targetPosition.x), (int)Mathf.Round(targetPosition.z));

        return _skill.GetSkillCoordinates(position, (int)Mathf.Round(_minBorderArea), (int)Mathf.Round(_maxBorderArea));
    }

    private  bool IsInRange(float value, float min, float max)
    {
        if (min > max)
            throw new ArgumentException("min должен быть <= max");

        return value >= min && value <= max;
    }
}
