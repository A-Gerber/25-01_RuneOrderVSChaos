using System;
using UnityEngine;

public abstract class Freezer : IEnemy
{
    private int _increaseToHealth;
    private int _maxHealth;
    private int _health;
    private float _skillCooldown;
    private Sprite _icon;
    private FreezingSkill _skill;

    public Freezer(float skillCooldown, Sprite icon, int healthIncrease)
    {
        if (skillCooldown <= 0)
            throw new ArgumentOutOfRangeException(nameof(skillCooldown));

        if (healthIncrease < 0)
            throw new ArgumentOutOfRangeException(nameof(healthIncrease));

        _icon = icon ?? throw new InvalidOperationException("icon is null");
        _skillCooldown = skillCooldown;
        _increaseToHealth = healthIncrease;
    }

    public event Action ChangedHealth;
    public event Action<IEnemySkill> UsedSkill;

    public bool IsAlive => _health > 0;
    public bool IsFullHealth => _maxHealth == _health;
    public int MaxHealth => _maxHealth;
    public int Health => _health;
    public int IncreaseToHealth => _increaseToHealth;
    public float SkillCooldown => _skillCooldown;
    public Sprite Icon => _icon;
    public Sprite SkillIcon => _skill.SkillIcon;
    public string SkillDescription => _skill.Description;

    public void TakeSkill(FreezingSkill freezingSkill)
    {
        _skill = freezingSkill ?? throw new InvalidOperationException("freezingSkill is null");
    }

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
        UpdateHealth();
        ChangedHealth?.Invoke();
    }

    public void UpdateHealth()
    {
        _health = _maxHealth;
        ChangedHealth?.Invoke();
    }

    public void SetMaxHealth(int health)
    {
        if (health <= 0)
            throw new ArgumentOutOfRangeException(nameof(health));

        _maxHealth = health;
        UpdateHealth();
    }

    public void TakeHealth(int health)
    {
        if (health < 0)
            throw new ArgumentOutOfRangeException(nameof(health));

        _health = Math.Min(_health + health, _maxHealth);
        ChangedHealth?.Invoke();
    }

    public void UseSkill()
    {
        UsedSkill?.Invoke(_skill);
    }
}

public class Fenrir : Freezer
{
    public Fenrir(float skillCooldown, Sprite icon, int healthIncrease) : base(skillCooldown, icon, healthIncrease)
    {
    }
}

public class Yeti : Freezer
{
    public Yeti(float skillCooldown, Sprite icon, int healthIncrease) : base(skillCooldown, icon, healthIncrease)
    {
    }
}

public class SnowQueen : Freezer
{
    public SnowQueen(float skillCooldown, Sprite icon, int healthIncrease) : base(skillCooldown, icon, healthIncrease)
    {
    }
}