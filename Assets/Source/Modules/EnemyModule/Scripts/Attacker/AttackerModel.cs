using System;
using UnityEngine;

public class AttackerModel : IConfigurableFromSkillSide
{
    private readonly ScoreCounter _ñounter;
    private readonly int _numberSimpleCombo;
    private IDamageable _enemy;
    private int _damagePerProjectile;

    public AttackerModel(ScoreCounter ñounter, int numberSimpleCombo)
    {
        if (numberSimpleCombo <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberSimpleCombo));

        _ñounter = ñounter ?? throw new InvalidOperationException("ñounter is null");
        _numberSimpleCombo = numberSimpleCombo;

        ñounter.SkillPointsAwarded += OnReward;

        Debug.Log("Ïîäóìàòü êàê îòïèñàòüñÿ");
    }

    public event Action<int> SkillPointsAwarded;
    internal event Action<int> CubesReleased;
    internal event Action UsedSkill;
    internal event Action ShakedCamera;

    public int MaxTotalCombo => _ñounter.MaxTotalCombo;

    public void SetEnemy(IDamageable enemy)
    {
        _enemy = enemy;
    }

    public void SetParameters(int damagePerProjectile, int comboSkillPointsInterval, float timeFrameOfCombo)
    {
        if (damagePerProjectile <= 0)
            throw new ArgumentOutOfRangeException(nameof(damagePerProjectile));

        _damagePerProjectile = damagePerProjectile;
        _ñounter.SetParameters(comboSkillPointsInterval, timeFrameOfCombo);
    }

    public void Attack(int countCells)
    {
        _enemy.TakeDamage(countCells * _damagePerProjectile);
        CubesReleased?.Invoke(countCells * _damagePerProjectile);

        int numberOfCombos = Mathf.CeilToInt(countCells / (float)Constants.AreaSize);

        _ñounter.CalculateCombo(numberOfCombos);

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
        _ñounter.ResetMaxScore();
    }

    private void OnReward(int count)
    {
        SkillPointsAwarded?.Invoke(count);
    }
}