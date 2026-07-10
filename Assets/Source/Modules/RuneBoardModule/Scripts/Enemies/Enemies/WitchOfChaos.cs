using System;
using System.Collections.Generic;
using UnityEngine;

public class WitchOfChaos : IEnemy
{
    private readonly int _increaseToHealth;
    private readonly float _skillCoolDown;
    private readonly Sprite _icon;
    private readonly List<IEnemySkill> _skills = new();

    private string _skillDescription;
    private int _maxHealth;
    private int _health;
    private HealingSkill _healingSkill;
    private Sprite _skillIcon;

    public WitchOfChaos(float skillCoolDown, Sprite icon, int healthIncrease)
    {
        if (skillCoolDown <= 0)
            throw new ArgumentOutOfRangeException(nameof(skillCoolDown));

        if (healthIncrease < 0)
            throw new ArgumentOutOfRangeException(nameof(healthIncrease));

        _icon = icon != null ? icon : throw new ArgumentNullException("icon is null", nameof(icon)); ;
        _skillCoolDown = skillCoolDown;
        _increaseToHealth = healthIncrease;

        ChangeSkillDescription(Constants.Language);
    }

    public event Action ChangedHealth;

    public bool IsAlive => _health > 0;
    public bool IsFullHealth => _maxHealth == _health;
    public int MaxHealth => _maxHealth;
    public int IncreaseToHealth => _increaseToHealth;
    public int Health => _health;
    public float SkillCoolDown => _skillCoolDown;
    public Sprite Icon => _icon;
    public Sprite SkillIcon => _skillIcon;

    public string SkillDescription => _skillDescription;

    public IEnemySkill EnemySkill => _skills[UnityEngine.Random.Range(0, _skills.Count)];

    public void TakeSkills(HealingSkill healingSkill, FreezingSkill freezingSkill, GroundImpact groundImpact, Sprite skillIcon)
    {
        if (freezingSkill == null)
            throw new InvalidOperationException("healingSkill is null");

        if (groundImpact == null)
            throw new InvalidOperationException("healingSkill is null");

        _healingSkill = healingSkill ?? throw new ArgumentNullException("healingSkill is null", nameof(healingSkill));
        _skillIcon = skillIcon != null ? skillIcon : throw new ArgumentNullException("skillIcon is null", nameof(skillIcon));

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

    public void ChangeSkillDescription(Languages language)
    {
        if (language == Languages.Russian)
        {
            _skillDescription = $"<color=#FFC300>Колдовство ведьмы <color=white>- позволяет использовать " +
                $"<color=#FFC300>Регенерацию<color=white>, <color=#FFC300>Метель<color=white>, <color=#FFC300>Каменные шипы<color=white>.";
        }
        else if (language == Languages.Turkish)
        {
            _skillDescription = $"<color=#FFC300>Cadının büyücülüğü - yenilenmeyi, kar fırtınasını ve taş dikenleri<color=white> kullanmanıza olanak tanır";
        }
        else
        {
            _skillDescription = $"<color=#FFC300>The witch's witchcraft <color=white>- allows you to use " +
                $"<color=#FFC300>Regeneration<color=white>, <color=#FFC300>Snowstorm<color=white>, <color=#FFC300>Stone spikes<color=white>.";
        }
    }
}