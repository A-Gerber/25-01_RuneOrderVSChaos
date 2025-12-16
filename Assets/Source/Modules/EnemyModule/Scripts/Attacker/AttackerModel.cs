using System;
using UnityEngine;

public class AttackerModel : IConfigurableFromSkillSide
{
    private readonly int _sizeOfLine;
    private IDamageable _enemy;
    private int _damagePerProjectile;
    private int _comboSkillPointsInterval;
    private float _timeFrameOfCombo;

    public AttackerModel(int sizeOfLine)
    {
        if (sizeOfLine <= 0)
            throw new ArgumentOutOfRangeException(nameof(sizeOfLine));

        _sizeOfLine = sizeOfLine;
    }

    public event Action<int> SkillPointsAwarded;
    internal event Action<int> FilledInLines;
    internal event Action<int> CubesReleased;
    internal event Action UsedSkill;
    internal event Action UpdatedParametrs;

    internal int ComboSkillPointsInterval => _comboSkillPointsInterval;
    internal float TimeFrameOfCombo => _timeFrameOfCombo;

    public void SetEnemy(IDamageable enemy)
    {
        _enemy = enemy;
    }

    public void SetParameters(int damagePerProjectile, int comboSkillPointsInterval, float timeFrameOfCombo)
    {
        if (damagePerProjectile <= 0)
            throw new ArgumentOutOfRangeException(nameof(damagePerProjectile));

        if (comboSkillPointsInterval <= 0)
            throw new ArgumentOutOfRangeException(nameof(comboSkillPointsInterval));

        if (timeFrameOfCombo <= 0)
            throw new ArgumentOutOfRangeException(nameof(timeFrameOfCombo));

        _damagePerProjectile = damagePerProjectile;
        _comboSkillPointsInterval = comboSkillPointsInterval;
        _timeFrameOfCombo = timeFrameOfCombo;

        UpdatedParametrs?.Invoke();
    }

    public void Attack(int countCells)
    {
        _enemy.TakeDamage(countCells * _damagePerProjectile);
        CubesReleased?.Invoke(countCells * _damagePerProjectile);
        FilledInLines?.Invoke((int)Mathf.Ceil(countCells / _sizeOfLine));
    }

    public void UseSkill(int countCells)
    {
        _enemy.TakeDamage(countCells * _damagePerProjectile);
        CubesReleased?.Invoke(countCells * _damagePerProjectile);
        UsedSkill?.Invoke();
    }

    internal void SendNumberOfSkillPoints(int count)
    {
        SkillPointsAwarded?.Invoke(count);
    }
}

public interface IConfigurableFromSkillSide
{
    void SetParameters(int damagePerProjectile, int comboSkillPointsInterval, float timeFrameOfCombo);
}