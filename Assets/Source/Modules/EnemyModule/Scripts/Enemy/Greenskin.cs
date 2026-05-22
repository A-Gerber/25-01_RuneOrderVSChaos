using System;
using UnityEngine;

public abstract class Greenskin : IEnemy
{
    private readonly int _increaseToHealth;
    private readonly float _skillCooldown;
    private readonly Sprite _icon;
    private HealingSkill _skill;
    private int _maxHealth;
    private int _health;

    public Greenskin(float skillCooldown, Sprite icon, int healthIncrease)
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

    public bool IsAlive => _health > 0;
    public bool IsFullHealth => _maxHealth == _health;
    public int MaxHealth => _maxHealth;
    public int IncreaseToHealth => _increaseToHealth;
    public int Health => _health;
    public float SkillCooldown => _skillCooldown;
    public Sprite Icon => _icon;
    public Sprite SkillIcon => _skill.SkillIcon;
    public string SkillDescription => _skill.Description;

    public void TakeSkill(HealingSkill healingSkill)
    {
        _skill = healingSkill ?? throw new InvalidOperationException("healingSkill is null");
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
        _skill.SetHealingValue(health);
        UpdateHealth();
    }

    public void TakeHealth(int health)
    {
        if (health < 0)
            throw new ArgumentOutOfRangeException(nameof(health));

        _health = Math.Min(_health + health, _maxHealth);
        ChangedHealth?.Invoke();
    }

    public IEnemySkill GetSkill()
    {
        return _skill;
    }

    public void ChangeSkillDescription(Languages language)
    {
        _skill.ChangeSkillDescription(language);
    }
}

public class Goblin : Greenskin
{
    public Goblin(float skillCooldown, Sprite icon, int healthIncrease) : base(skillCooldown, icon, healthIncrease)
    {
    }
}

public class Orc : Greenskin
{
    public Orc(float skillCooldown, Sprite icon, int healthIncrease) : base(skillCooldown, icon, healthIncrease)
    {
    }
}

public class OrcChieftain : Greenskin
{
    public OrcChieftain(float skillCooldown, Sprite icon, int healthIncrease) : base(skillCooldown, icon, healthIncrease)
    {
    }
}
