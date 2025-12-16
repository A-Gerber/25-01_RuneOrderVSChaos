using System;
using UnityEngine;

public class SimpleEnemyModel : IEnemy
{
    private readonly int _maxHealth;
    private int _health;
    private float _skillCooldown;
    private Sprite _icon;

    public SimpleEnemyModel(int health, float skillCooldown, Sprite icon)
    {
        if (health <= 0)
            throw new ArgumentOutOfRangeException(nameof(health));

        if (skillCooldown <= 0)
            throw new ArgumentOutOfRangeException(nameof(skillCooldown));

        _icon = icon ?? throw new InvalidOperationException("icon is null");
        _skillCooldown = skillCooldown;
        _maxHealth = health;
        SetMaxHealth();
    }

    internal event Action ChangedHealth;

    internal int MaxHealth => _maxHealth;
    internal float SkillCooldown => _skillCooldown;
    internal Sprite Icon => _icon;
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

            ChangedHealth?.Invoke();
        }
    }

    public void Restart()
    {
        SetMaxHealth();
        ChangedHealth?.Invoke();
    }

    internal void SetMaxHealth()
    {
        _health = _maxHealth;
        ChangedHealth?.Invoke();
    }
}