using System;
using System.Collections.Generic;
using UnityEngine;

public class UserSkillPerformer : ISubscribeable
{
    private readonly Pusher _pusher;
    private IUseableUserSkills _area;
    private ICreateableCubesForArea _cubeViewSpawner;
    private UserSkill _skill;
    private bool _isPressedButton = false;
    private float _forceImpact;

    public UserSkillPerformer(Pusher pusher, float forceImpact)
    {
        if (forceImpact <= 0)
            throw new ArgumentOutOfRangeException(nameof(forceImpact));

        _pusher = pusher ?? throw new InvalidOperationException("pusher is null");
        _forceImpact = forceImpact;
    }

    internal event Action EnabledAttackZone;
    internal event Action DisabledAttackZone;

    public bool IsPressedButton => _isPressedButton;
    public int CurrentManaCost => _skill.ManaCost;

    public void Initialize(IUseableUserSkills area, ICreateableCubesForArea cubeViewSpawner)
    {
        _area = area ?? throw new InvalidOperationException("area is null");
        _cubeViewSpawner = cubeViewSpawner ?? throw new InvalidOperationException("cubeViewSpawner is null");
    }

    public void FillCoordinatesOfArrow(Arrow arrow)
    {
        FillAreaByCoordinate(arrow.Coordinates);
        arrow.Activating -= FillCoordinatesOfArrow;
    }

    public void SubscribeToArrow(Arrow arrow)
    {
        arrow.Activating += FillCoordinatesOfArrow;
    }

    public void PressButton(UserSkill skill)
    {
        _skill = skill ?? throw new InvalidOperationException("skill is null");
        _isPressedButton = true;

        EnabledAttackZone?.Invoke();
    }

    public bool TryUseSkill()
    {
        _isPressedButton = false;
        DisabledAttackZone?.Invoke();

        Vector3 targetPosition = UserUtilities.GetCursorPosition(Constants.CameraHeight);

        if (UserUtilities.IsLocateInArena(targetPosition) == false)
            return false;

        if (_skill is ISetableInThirdButton)
        {
            _skill.Use(targetPosition);
            _area.SetCountTargetDamage(_skill.SkillDamage);
            return true;
        }

        LocalPosition position = new LocalPosition((int)Mathf.Round(targetPosition.x), (int)Mathf.Round(targetPosition.z));
        List<LocalPosition> skillCoordinates = _skill.GetSkillCoordinates(position, (int)Mathf.Round(Constants.MinBorderArea), (int)Mathf.Round(Constants.MaxBorderArea));      

        if (_skill is ISetableInFirstButton)
        {
            if (_area.TryFindTargetsForStrike(skillCoordinates, out List<Cube> targets))
            {
                _pusher.Push(targets, targetPosition, _forceImpact);
                _skill.Use(targetPosition);
                return true;
            }
        }
        else if (_skill is ISetableInSecondButton)
        {
            FillAreaByCoordinate(skillCoordinates);
            _area.TryFindTargetCellsByLines();
            _skill.Use(targetPosition);
            return true;
        }

        return false;
    }

    private void FillAreaByCoordinate(IReadOnlyList<LocalPosition> skillCoordinates)
    {
        List<CellModel> cells = _area.GetCellsForFilling(out List<LocalPosition> cellCoordinates, skillCoordinates);

        _cubeViewSpawner.CreateCubesForArea(cellCoordinates, cells);
    }
}