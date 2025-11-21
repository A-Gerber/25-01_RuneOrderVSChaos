using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillUser
{
    private Skill _skill;
    private float _minBorderArea;
    private float _maxBorderArea;
    private float _height;
    private bool _isPressedButton = false;

    public SkillUser(Skill skill, float minBorderArea, float maxBorderArea, float height)
    {
        _skill = skill ?? throw new InvalidOperationException("skill is null");

        _minBorderArea = minBorderArea;
        _maxBorderArea = maxBorderArea;
        _height = height;
    }

    internal event Action EnabledAttackZone;
    internal event Action DisabledAttackZone;

    public bool IsPressedButton => _isPressedButton;
    internal float Height => _height;

    public bool TryGetSkillCoordinates(out List<LocalPosition> coordinates, int skillCount)
    {
        coordinates = new List<LocalPosition>();
        _isPressedButton = false;

        Vector3 targetPosition = UserUtilities.GetCursorPosition(_height);

        if (UserUtilities.IsInRange(targetPosition.x, _minBorderArea, _maxBorderArea) == false || UserUtilities.IsInRange(targetPosition.z, _minBorderArea, _maxBorderArea) == false)
        {
            DisabledAttackZone?.Invoke();
            return false;
        }

        coordinates = GetSkillCoordinates(targetPosition);
        return true;
    }

    public void UseSkill()
    {
        DisabledAttackZone?.Invoke();
        _skill.Use();
    }

    public void PressButton()
    {
        _isPressedButton = true;
        EnabledAttackZone?.Invoke();
    }

    private List<LocalPosition> GetSkillCoordinates(Vector3 targetPosition)
    {
        LocalPosition position = new LocalPosition((int)Mathf.Round(targetPosition.x), (int)Mathf.Round(targetPosition.z));

        return _skill.GetSkillCoordinates(position, (int)Mathf.Round(_minBorderArea), (int)Mathf.Round(_maxBorderArea));
    }
}
