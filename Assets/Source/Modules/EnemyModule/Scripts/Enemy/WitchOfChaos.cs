using System;
using System.Collections.Generic;
using UnityEngine;

public class WitchOfChaos : IEnemy
{
    private readonly string _skillDescription;
    private readonly int _increaseToHealth;
    private readonly float _skillCooldown;
    private readonly Sprite _icon;
    private int _maxHealth;
    private int _health;
    private List<IEnemySkill> _skills = new();
    private HealingSkill _healingSkill;
    private Sprite _skillIcon;

    public WitchOfChaos(float skillCooldown, Sprite icon, int healthIncrease)
    {
        if (skillCooldown <= 0)
            throw new ArgumentOutOfRangeException(nameof(skillCooldown));

        if (healthIncrease < 0)
            throw new ArgumentOutOfRangeException(nameof(healthIncrease));

        _icon = icon ?? throw new InvalidOperationException("icon is null");
        _skillCooldown = skillCooldown;
        _increaseToHealth = healthIncrease;
        _skillDescription = $"<color=#FFC300>The witch's witchcraft <color=white>- allows you to use " +
            $"<color=#FFC300>Regeneration<color=white>, <color=#FFC300>Snowstorm<color=white>, <color=#FFC300>Stone spikes<color=white>.";
    }

    public event Action ChangedHealth;
    public event Action<IEnemySkill> UsedSkill;

    public bool IsAlive => _health > 0;
    public bool IsFullHealth => _maxHealth == _health;
    public int MaxHealth => _maxHealth;
    public int IncreaseToHealth => _increaseToHealth;
    public int Health => _health;
    public float SkillCooldown => _skillCooldown;
    public Sprite Icon => _icon;
    public Sprite SkillIcon => _skillIcon;

    public string SkillDescription => _skillDescription;

    public void TakeSkills(HealingSkill healingSkill, FreezingSkill freezingSkill, GroundImpact groundImpact, Sprite skillIcon)
    {
        if (freezingSkill == null)
            throw new InvalidOperationException("healingSkill is null");

        if (groundImpact == null)
            throw new InvalidOperationException("healingSkill is null");

        _healingSkill = healingSkill ?? throw new InvalidOperationException("healingSkill is null");
        _skillIcon = skillIcon != null ? skillIcon : throw new InvalidOperationException("skillIcon is null");

        _skills.Add(_healingSkill);
        _skills.Add(freezingSkill);
        _skills.Add(groundImpact);
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
        _healingSkill.SetHealingValue(health);

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
        int index = UnityEngine.Random.Range(0, _skills.Count);

        UsedSkill?.Invoke(_skills[index]);
    }
}