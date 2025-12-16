using System;
using System.Collections.Generic;
using UnityEngine;

public class UserSkillPerformer
{
    private IUseableUserSkills _area;
    private ICreateableCubesForArea _cubeViewSpawner;
    private UserSkill _skill;
    private float _minBorderArea;
    private float _maxBorderArea;
    private float _height;
    private bool _isPressedButton = false;

    public UserSkillPerformer(float minBorderArea, float maxBorderArea, float height)
    {
        _minBorderArea = minBorderArea;
        _maxBorderArea = maxBorderArea;
        _height = height;
    }

    internal event Action<ParticleSystem> EnabledAttackZone;
    internal event Action<ParticleSystem> DisabledAttackZone;

    public bool IsPressedButton => _isPressedButton;
    internal float Height => _height;

    public void Initialize(IUseableUserSkills area, ICreateableCubesForArea cubeViewSpawner)
    {
        _area = area ?? throw new InvalidOperationException("area is null");
        _cubeViewSpawner = cubeViewSpawner ?? throw new InvalidOperationException("cubeViewSpawner is null");
    }

    public void PressButton(UserSkill skill)
    {
        _skill = skill ?? throw new InvalidOperationException("skill is null");
        _isPressedButton = true;

        EnabledAttackZone?.Invoke(_skill.AttackZone);
    }

    public bool TryUseSkill()
    {
        _isPressedButton = false;
        DisabledAttackZone?.Invoke(_skill.AttackZone);

        Vector3 targetPosition = UserUtilities.GetCursorPosition(_height);

        if (IsClickedInArena(targetPosition))
            return false;

        if (_skill is ISetableInThirdButton)
        {
            _area.SetCountTargetDamage(_skill.SkillDamage);
            return true;
        }

        LocalPosition position = new LocalPosition((int)Mathf.Round(targetPosition.x), (int)Mathf.Round(targetPosition.z));
        List<LocalPosition> skillCoordinates = _skill.GetSkillCoordinates(position, (int)Mathf.Round(_minBorderArea), (int)Mathf.Round(_maxBorderArea));      

        if (_skill is ISetableInFirstButton)
        {
            if (_area.TryFindTargetCellsForStrike(skillCoordinates))
            {
                _skill.Use();
                return true;
            }
        }
        else if (_skill is ISetableInSecondButton)
        {
            List<CellModel> cells = _area.GetCellsForFilling(out List<LocalPosition> cellCoordinates, skillCoordinates);

            _cubeViewSpawner.CreateCubesForArea(cellCoordinates, cells);
            _area.TryFindTargetCellsByLines();
            _skill.Use();
            return true;
        }

        return false;
    } 
    
    private bool IsClickedInArena(Vector3 targetPosition)
    {
        bool isAbscissaOutsideArea = UserUtilities.IsInRange(targetPosition.x, _minBorderArea, _maxBorderArea) == false;
        bool isApplicateOutsideArea = UserUtilities.IsInRange(targetPosition.z, _minBorderArea, _maxBorderArea) == false;

        return isAbscissaOutsideArea || isApplicateOutsideArea;
    }
}