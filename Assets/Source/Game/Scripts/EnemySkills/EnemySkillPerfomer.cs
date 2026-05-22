using System;
using System.Collections.Generic;
using UnityEngine;

internal class EnemySkillPerfomer
{
    private readonly IUseableEnemySkills _areaModel;
    private readonly StalactiteViewSpawner _stalactiteSpawner;
    private readonly GroundImpactEffectSpawner _groundImpactEffectSpawner;
    private readonly FreezingEffectSpawner _freezingEffectSpawner;
    private IEnemy _enemy;
    private ITakeable _stalactiteTargetCell;

    internal EnemySkillPerfomer(IUseableEnemySkills areaModel, StalactiteViewSpawner stalactiteSpawner, GroundImpactEffectSpawner groundImpactEffectSpawner, FreezingEffectSpawner freezingEffectSpawner)
    {
        _areaModel = areaModel ?? throw new InvalidOperationException("areaModel is null");
        _stalactiteSpawner = stalactiteSpawner != null ? stalactiteSpawner : throw new InvalidOperationException("stalactiteSpawner is null");
        _groundImpactEffectSpawner = groundImpactEffectSpawner != null ? groundImpactEffectSpawner : throw new InvalidOperationException("groundImpactEffectSpawner is null");
        _freezingEffectSpawner = freezingEffectSpawner != null ? freezingEffectSpawner : throw new InvalidOperationException("freezingEffectSpawner is null");

        _stalactiteSpawner.GetedStalactiteView += OnGetStalactiteView;
        Debug.Log("Подумать как отписаться");
    }

    internal event Action<int> UsedHealingSkill;
    internal event Action<Vector3> UsedFreezingSkill;
    internal event Action<Vector3> UsedGroundImpact;
    internal event Action PlacedStalactite;
    internal event Action Initialized;

    internal bool CanUseSkill => _enemy != null && _enemy.IsAlive;
    internal float EnemySkillCooldown => _enemy.SkillCooldown;

    internal void Initialize(IEnemy enemy)
    {
        _enemy = enemy ?? throw new InvalidOperationException("enemy is null");
        Initialized?.Invoke();
    }

    internal void UseSkill()
    {
        IEnemySkill skill = _enemy.GetSkill() ?? throw new InvalidOperationException("skill is null");

        if (skill is HealingSkill healingSkill)
        {
            UseHealingSkill(healingSkill);
        }
        else if (skill is FreezingSkill freezingSkill)
        {
            UseFreezingSkill(freezingSkill);
        }
        else if (skill is GroundImpact groundImpact)
        {
            UseGroundImpact(groundImpact);
        }
    }

    private void UseHealingSkill(HealingSkill healingSkill)
    {
        if (_enemy.IsFullHealth == false)
        {
            if (_enemy.MaxHealth - _enemy.Health < healingSkill.HealingValue)
                healingSkill.SetDisplayedHealingValue(_enemy.MaxHealth - _enemy.Health);
            else
                healingSkill.SetDisplayedHealingValue(healingSkill.HealingValue);

            _enemy.TakeHealth(healingSkill.HealingValue);
            UsedHealingSkill?.Invoke(healingSkill.DisplayedHealingValue);
        }
    }

    private void UseFreezingSkill(FreezingSkill freezingSkill)
    {
        for (int i = 0; i < freezingSkill.NumberOfUses; i++)
        {
            Vector3 position = Vector3.zero;

            if (_areaModel.TryFreezeRandomShape(ref position))
            {
                _freezingEffectSpawner.CreateEffect(position);
                UsedFreezingSkill?.Invoke(position);
            }
        }
    }

    private void UseGroundImpact(GroundImpact groundImpact)
    {
        for (int i = 0; i < groundImpact.NumberOfUses; i++)
        {
            if (TryGetRandomFreeCells())
            {
                Vector3 position = new(_stalactiteTargetCell.Position.PositionX, 0f, _stalactiteTargetCell.Position.PositionZ);
                _groundImpactEffectSpawner.CreateEffect(position);
                _stalactiteSpawner.CreateStalactite(position);
                UsedGroundImpact?.Invoke(position);
            }
        }
    }

    private void OnGetStalactiteView(StalactiteView view)
    {
        _stalactiteTargetCell.Take(view.GetStalactite());
        PlacedStalactite?.Invoke();
    }

    private bool TryGetRandomFreeCells()
    {
        List<ITakeable> cells = new();

        for (int i = 0; i < Constants.AreaSize; i++)
        {
            for (int j = 0; j < Constants.AreaSize; j++)
            {
                if (_areaModel.TryGetCellByCoordinate(out ITakeable cell, new LocalPosition(i, j)) && cell.IsBusy == false)
                    cells.Add(cell);
            }
        }

        bool canGet = cells.Count > 0;

        if (canGet)
            _stalactiteTargetCell = cells[UnityEngine.Random.Range(0, cells.Count)];

        return canGet;
    }
}