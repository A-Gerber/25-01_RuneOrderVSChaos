using System;
using System.Collections.Generic;
using UnityEngine;

internal class EnemySkillPerfomer
{
    private readonly ICellGetable _playField;
    private readonly IShapeFreezable _shapePlatform;
    private readonly StalactiteViewSpawner _stalactiteSpawner;
    private readonly GroundImpactEffectSpawner _groundImpactEffectSpawner;
    private readonly FreezingEffectSpawner _freezingEffectSpawner;
    private readonly FrozenState _frozenState = new(true);

    private IEnemy _enemy;
    private ITakeable _stalactiteTargetCell;

    internal EnemySkillPerfomer(IShapeFreezable shapePlatform, ICellGetable playField, EnemyEffectSpawner enemyEffectSpawners)
    {
        _playField = playField ?? throw new ArgumentNullException("playField is null", nameof(playField));
        _shapePlatform = shapePlatform ?? throw new ArgumentNullException("shapePlatform is null", nameof(shapePlatform));
        _stalactiteSpawner = enemyEffectSpawners.StalactiteSpawner != null ? enemyEffectSpawners.StalactiteSpawner : throw new ArgumentNullException("StalactiteSpawner is null", nameof(enemyEffectSpawners.StalactiteSpawner));
        _groundImpactEffectSpawner = enemyEffectSpawners.GroundImpactEffectSpawner != null ? enemyEffectSpawners.GroundImpactEffectSpawner : throw new ArgumentNullException("GroundImpactEffectSpawner is null", nameof(enemyEffectSpawners.GroundImpactEffectSpawner));
        _freezingEffectSpawner = enemyEffectSpawners.FreezingEffectSpawner != null ? enemyEffectSpawners.FreezingEffectSpawner : throw new ArgumentNullException("FreezingEffectSpawner is null", nameof(enemyEffectSpawners.FreezingEffectSpawner));

        _stalactiteSpawner.GetedStalactiteView += OnGetStalactiteView;
    }

    internal event Action<int> UsedHealingSkill;
    internal event Action UsedSkill;
    internal event Action PlacedStalactite;
    internal event Action Initialized;

    internal bool CanUseSkill => _enemy != null && _enemy.IsAlive;
    internal float EnemySkillCooldown => _enemy.SkillCoolDown;

    internal void Initialize(IEnemy enemy)
    {
        _enemy = enemy ?? throw new InvalidOperationException("enemy is null");
        Initialized?.Invoke();
    }

    internal void Unsubscribe()
    {
        _stalactiteSpawner.GetedStalactiteView -= OnGetStalactiteView;
    }

    internal void UseSkill()
    {
        IEnemySkill skill = _enemy.EnemySkill ?? throw new InvalidOperationException("skill is null");

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

            if (_shapePlatform.TryFreezeRandomShape(ref position, _frozenState))
            {
                _freezingEffectSpawner.CreateEffect(position);
                UsedSkill?.Invoke();
            }
        }
    }

    private void UseGroundImpact(GroundImpact groundImpact)
    {
        for (int i = 0; i < groundImpact.NumberOfUses; i++)
        {
            if (TryGetRandomFreeCells())
            {
                Vector3 position = new(_stalactiteTargetCell.Position.X, 0f, _stalactiteTargetCell.Position.Z);
                _groundImpactEffectSpawner.CreateEffect(position);
                _stalactiteSpawner.CreateStalactite(position);
                UsedSkill?.Invoke();
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
                if (_playField.TryGetCellByPosition(out ITakeable cell, new LocalPosition(i, j)) && cell.IsBusy == false)
                    cells.Add(cell);
            }
        }

        if (cells.Count == 0)
            return false;


        _stalactiteTargetCell = cells[UnityEngine.Random.Range(0, cells.Count)];
        return true;
    }
}