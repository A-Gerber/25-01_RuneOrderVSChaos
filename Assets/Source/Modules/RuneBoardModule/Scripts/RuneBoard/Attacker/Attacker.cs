using System;
using System.Collections.Generic;
using UnityEngine;

public class Attacker: IAttackerSkillContactable
{
    private const int ScoreMultiplier = 10;

    private readonly IGetableEnemy _enemiesFactory;
    private readonly ProjectileSpawner _projectileSpawner;
    private readonly ScoreCounter _ñounter;

    private IChangeableHealthEnemy _enemy;
    private int _damagePerProjectile = 1;
    private int _rewardMultiplier;
    private int _bulletCount;

    internal Attacker(IGetableEnemy enemiesFactory, ProjectileSpawner projectileSpawner, ScoreCounter ñounter)
    {
        _enemiesFactory = enemiesFactory ?? throw new ArgumentNullException("enemiesFactory is null", nameof(enemiesFactory));
        _ñounter = ñounter ?? throw new ArgumentNullException("ñounter is null", nameof(ñounter));
        _projectileSpawner = projectileSpawner != null ? projectileSpawner : throw new ArgumentNullException("projectileSpawner is null", nameof(projectileSpawner));

        ñounter.SkillPointsAwarded += OnReward;
    }

    public event Action<int> RewardingManaUserSkillPerformer;
    internal event Action<int> Damaged;
    internal event Action ShakedCamera;
    internal event Action ChangeMultiplier;
    internal event Action CubesReleased;

    internal int DamageMultiplier => _damagePerProjectile + _rewardMultiplier;
    internal int ScoreIncrease => _ñounter.MaxTotalCombo * ScoreMultiplier + _enemy.MaxHealth;
    internal bool IsAliveEnemy => _enemy.IsAlive;
    internal bool CanAttack { get; private set; } = true;

    public void SetParameters(int damagePerProjectile, int comboSkillPointsInterval, float timeFrameOfCombo)
    {
        if (damagePerProjectile <= 0)
            throw new ArgumentOutOfRangeException(nameof(damagePerProjectile));

        _damagePerProjectile = damagePerProjectile;
        _ñounter.SetParameters(comboSkillPointsInterval, timeFrameOfCombo);
        ChangeMultiplier?.Invoke();
    }

    public void SetRewardMultiplier(int value)
    {
        _rewardMultiplier += value;
        ChangeMultiplier?.Invoke();
    }

    public void DamageWithSkill(int count)
    {
        _enemy.TakeDamage(count * DamageMultiplier);
        Damaged?.Invoke(count * DamageMultiplier);
        ShakedCamera?.Invoke();
    }

    internal void Start(int level)
    {
        _enemy = _enemiesFactory.GetEnemy(level);
        _ñounter.ResetMaxScore();
        _rewardMultiplier = 0;
        ChangeMultiplier?.Invoke();
    }

    internal void Restart()
    {
        _enemy.Restart();
        _ñounter.ResetMaxScore();
        _rewardMultiplier = 0;
        ChangeMultiplier?.Invoke();
    }

    internal void Damage()
    {
        CanAttack = false;
       
        _enemy.TakeDamage(_bulletCount * DamageMultiplier);
        Damaged?.Invoke(_bulletCount * DamageMultiplier);

        int numberOfCombos = Mathf.CeilToInt(_bulletCount / (float)Constants.AreaSize);
        _ñounter.CalculateCombo(numberOfCombos);

        if (numberOfCombos > _ñounter.NumberSimpleCombo)
            ShakedCamera?.Invoke();

        CubesReleased?.Invoke();
    }

    internal void Attack(List<LocalPosition> positions)
    {
        CanAttack = true;
        _bulletCount = positions.Count;
        _projectileSpawner.CreateBullets(positions);
    }

    private void OnReward(int numberOfRewards)
    {
        RewardingManaUserSkillPerformer?.Invoke(numberOfRewards);
    }
}