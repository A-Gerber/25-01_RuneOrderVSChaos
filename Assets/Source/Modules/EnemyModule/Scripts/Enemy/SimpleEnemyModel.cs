using System;
using UnityEngine;

public class SimpleEnemyModel : IEnemy
{
    private int _health;

    public SimpleEnemyModel(int health, Vector3 position)
    {
        if (health <= 0)
            throw new ArgumentOutOfRangeException(nameof(health));

        MaxHealth = health;
        SetMaxHealth();
        Position = position;
    }

    internal event Action ChangedHealth;

    internal int MaxHealth { get; private set; }
    internal Vector3 Position { get; private set; }
    internal int Health => _health;
    public bool IsAlive => _health > 0;

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        if (IsAlive)
        {
            _health -= damage;

            if (_health < 0)
                _health = 0;
        }
    }

    public void Restart()
    {
        SetMaxHealth();
        ChangedHealth?.Invoke();
    }

    internal void SetMaxHealth()
    {
        _health = MaxHealth;
    }
}