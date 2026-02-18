using System;
using UnityEngine;

public class AttackerModel : IConfigurableFromSkillSide
{

    private readonly Score—ounter _Òounter;
    private readonly int _numberSimpleCombo;
    private IDamageable _enemy;
    private int _damagePerProjectile;

    public AttackerModel(Score—ounter Òounter, int numberSimpleCombo)
    {
        if (numberSimpleCombo <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberSimpleCombo));

        _Òounter = Òounter ?? throw new InvalidOperationException("Òounter is null");
        _numberSimpleCombo = numberSimpleCombo;

        Òounter.SkillPointsAwarded += OnReward;
    }

    public event Action<int> SkillPointsAwarded;
    internal event Action<int> CubesReleased;
    internal event Action UsedSkill;
    internal event Action ShakedCamera;

    public int MaxTotalCombo => _Òounter.MaxTotalCombo;

    public void SetEnemy(IDamageable enemy)
    {
        _enemy = enemy;
    }

    public void SetParameters(int damagePerProjectile, int comboSkillPointsInterval, float timeFrameOfCombo)
    {
        if (damagePerProjectile <= 0)
            throw new ArgumentOutOfRangeException(nameof(damagePerProjectile));

        _damagePerProjectile = damagePerProjectile;
        _Òounter.SetParameters(comboSkillPointsInterval, timeFrameOfCombo);
    }

    public void Attack(int countCells)
    {
        _enemy.TakeDamage(countCells * _damagePerProjectile);
        CubesReleased?.Invoke(countCells * _damagePerProjectile);

        int numberOfCombos = (int)Mathf.Ceil(countCells / UserUtilities.AreaSize);
        _Òounter.CalculateCombo(numberOfCombos);

        if (numberOfCombos > _numberSimpleCombo)
            ShakedCamera?.Invoke();
    }

    public void UseSkill(int countCells)
    {
        _enemy.TakeDamage(countCells * _damagePerProjectile);
        CubesReleased?.Invoke(countCells * _damagePerProjectile);
        UsedSkill?.Invoke();
    }


    public void ResetCounter()
    {
        _Òounter.ResetMaxScore();
    }

    private void OnReward(int count)
    {
        SkillPointsAwarded?.Invoke(count);
    }
}