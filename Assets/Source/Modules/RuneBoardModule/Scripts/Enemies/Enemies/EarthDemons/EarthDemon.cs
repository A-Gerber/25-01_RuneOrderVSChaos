using System;
using UnityEngine;

public abstract class EarthDemon : IEnemy
{
    private readonly int _increaseToHealth;
    private readonly float _skillCoolDown;
    private readonly Sprite _icon;

    private int _maxHealth;
    private int _health;
    private GroundImpact _skill;

    public EarthDemon(float skillCoolDown, Sprite icon, int healthIncrease)
    {
        if (skillCoolDown <= 0)
            throw new ArgumentOutOfRangeException(nameof(skillCoolDown));

        if (healthIncrease <= 0)
            throw new ArgumentOutOfRangeException(nameof(healthIncrease));

        _icon = icon != null ? icon : throw new ArgumentNullException("icon is null", nameof(icon));
        _skillCoolDown = skillCoolDown;
        _increaseToHealth = healthIncrease;
    }

    public event Action ChangedHealth;

    public bool IsAlive => _health > 0;
    public bool IsFullHealth => _maxHealth == _health;
    public int MaxHealth => _maxHealth;
    public int IncreaseToHealth => _increaseToHealth;
    public int Health => _health;
    public float SkillCoolDown => _skillCoolDown;
    public Sprite Icon => _icon;
    public Sprite SkillIcon => _skill.SkillIcon;
    public string SkillDescription => _skill.Description;
    public IEnemySkill EnemySkill => _skill;

    public void TakeSkill(GroundImpact groundImpact)
    {
        _skill = groundImpact ?? throw new ArgumentNullException("groundImpact is null", nameof(groundImpact));
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

    public void ChangeSkillDescription(Languages language)
    {
        _skill.ChangeSkillDescription(language);
    }
}