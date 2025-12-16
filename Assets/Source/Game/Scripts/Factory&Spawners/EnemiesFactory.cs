using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class EnemiesFactory : MonoBehaviour
{
    [SerializeField] private EnemyPresenter _enemyPresenter;
    [SerializeField] private int _startEnemyHealth = 50;
    [SerializeField] private int _increase = 20;
    [SerializeField] private int _divider = 3;
    [SerializeField] private int _skillCooldown = 3;
    [SerializeField] private Sprite _simpleEnemy;

    internal Vector3 GetEnemyPosition()
    {
        return _enemyPresenter.transform.position;
    }

    internal IEnemy Create(int level)
    {
        int health = CalculateHealth(level);
        SimpleEnemyModel enemy = new SimpleEnemyModel(health, _skillCooldown, _simpleEnemy);
        _enemyPresenter.SetEnemy(enemy);

        return enemy;
    }

    private int CalculateHealth(int level)
    {
        if (level <= 0)
            throw new ArgumentOutOfRangeException(nameof(level));

        int coefficient = level / _divider;

        return _startEnemyHealth + _increase * level + coefficient * _increase;
    }
}