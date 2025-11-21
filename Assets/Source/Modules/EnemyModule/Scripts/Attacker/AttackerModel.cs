using System;
using UnityEngine;

public class AttackerModel
{
    private readonly int _damagePerProjectile;
    private IDamageable _enemy;
    private int _sizeOfLine;

    public AttackerModel(int damagePerProjectile, int sizeOfLine)
    {
        if (damagePerProjectile <= 0)
            throw new ArgumentOutOfRangeException(nameof(damagePerProjectile));

        if (sizeOfLine <= 0)
            throw new ArgumentOutOfRangeException(nameof(sizeOfLine));

        _damagePerProjectile = damagePerProjectile;
        _sizeOfLine = sizeOfLine;
    }

    public event Action<int> SkillPointsAwarded;
    internal event Action<int> FilledInLines;
    internal event Action<int> CubesReleased;
    internal event Action UsedSkill;

    public void SetEnemy(IDamageable enemy)
    {
        _enemy = enemy;
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