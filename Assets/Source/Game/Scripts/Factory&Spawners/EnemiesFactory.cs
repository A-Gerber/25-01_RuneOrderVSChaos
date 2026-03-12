using System;
using System.Collections.Generic;
using UnityEngine;

internal class EnemiesFactory : MonoBehaviour
{
    [SerializeField] private EnemySkillFactory _enemySkillFactory;
    [SerializeField] private EnemyPresenter _enemyPresenter;
    [SerializeField] private EnemySkillPerfomerView _enemySkillPerfomerView;
    [SerializeField] private int _startEnemyHealth = 65;
    [SerializeField] private int _increasePerLevel = 3;
    [SerializeField] private int _divider = 20;
    [SerializeField] private int _powerOfCoefficient = 3;
    [SerializeField] private int _powerMultiplier  = 20;

    [Header("EnemyParameters")]
    [SerializeField] private float _goblinSkillCooldown = 180f;
    [SerializeField] private int _goblinIncreaseToHealth = 0;
    [SerializeField] private Sprite _goblinIcon;

    [SerializeField] private float _orcSkillCooldown = 25f;
    [SerializeField] private int _orcIncreaseToHealth = 15;
    [SerializeField] private Sprite _orcIcon;

    [SerializeField] private float _orcChieftainSkillCooldown = 20f;
    [SerializeField] private int _orcChieftainIncreaseToHealth = 30;
    [SerializeField] private Sprite _orcChieftainIcon;

    [SerializeField] private float _yetiSkillCooldown = 15f;
    [SerializeField] private int _yetiIncreaseToHealth = 0;
    [SerializeField] private Sprite _yetiIcon;

    [SerializeField] private float _fenrirSkillCooldown = 10f;
    [SerializeField] private int _fenrirIncreaseToHealth = 20;
    [SerializeField] private Sprite _fenrirIcon;

    [SerializeField] private float _snowQueenSkillCooldown = 15f;
    [SerializeField] private int _snowQueenIncreaseToHealth = 25;
    [SerializeField] private Sprite _snowQueenIcon;

    [SerializeField] private float _gargoyleSkillCooldown = 25f;
    [SerializeField] private int _gargoyleIncreaseToHealth = 10;
    [SerializeField] private Sprite _gargoyleIcon;

    [SerializeField] private float _earthDragonSkillCooldown = 20f;
    [SerializeField] private int _earthDragonIncreaseToHealth = 20;
    [SerializeField] private Sprite _earthDragonIcon;

    [SerializeField] private float _witchSkillCooldown = 12f;
    [SerializeField] private int _witchIncreaseToHealth = 175;
    [SerializeField] private Sprite _witchIcon;

    private EnemiesGenerator _generator;
    private EnemySkillPerfomer _enemySkillPerfomer;

    internal Vector3 GetEnemyPosition()
    {
        return _enemyPresenter.transform.position;
    }

    internal void SetGameForEnemyPresenter(IPerformableAttack game)
    {
        _enemyPresenter.SetGame(game);
    }

    internal EnemySkillPerfomer Create(AreaModel areaModel, StalactiteViewSpawner stalactiteSpawner, GroundImpactEffectSpawner groundImpactEffectSpawner, FreezingEffectSpawner freezingEffectSpawner)
    {
        groundImpactEffectSpawner.SetStartPosition(GetEnemyPosition());

        _generator = new EnemiesGenerator(CreateEnemies());
        _enemySkillPerfomer = new EnemySkillPerfomer(areaModel, stalactiteSpawner, groundImpactEffectSpawner, freezingEffectSpawner);
        _enemySkillPerfomerView.Initialize(_enemySkillPerfomer);

        return _enemySkillPerfomer;
    }

    internal IChangeableHealthEnemy GetEnemy(int level)
    {
        IEnemy enemy = _generator.Generate(level);
        int health = CalculateHealth(level, enemy);
        enemy.SetMaxHealth(health);
        _enemyPresenter.SetEnemy(enemy);
        _enemySkillPerfomer.SubscribeToEnemy(enemy);

        return enemy;
    }

    private int CalculateHealth(int level, IEnemy enemy)
    {
        if (level < 0)
            throw new ArgumentOutOfRangeException(nameof(level));

        int coefficient = level / _divider;

        return _startEnemyHealth + enemy.IncreaseToHealth + _increasePerLevel * level + _powerMultiplier * (int)Mathf.Pow(coefficient, _powerOfCoefficient);
    }

    private List<IEnemy> CreateEnemies()
    {
        List<IEnemy> enemies = new()
        {
            new Goblin(_goblinSkillCooldown, _goblinIcon, _goblinIncreaseToHealth),
            new Orc(_orcSkillCooldown, _orcIcon, _orcIncreaseToHealth),
            new Yeti(_yetiSkillCooldown, _yetiIcon, _yetiIncreaseToHealth),
            new OrcChieftain(_orcChieftainSkillCooldown, _orcChieftainIcon, _orcChieftainIncreaseToHealth),
            new Fenrir(_fenrirSkillCooldown, _fenrirIcon, _fenrirIncreaseToHealth),
            new Gargoyle(_gargoyleSkillCooldown, _gargoyleIcon, _gargoyleIncreaseToHealth),
            new SnowQueen(_snowQueenSkillCooldown, _snowQueenIcon, _snowQueenIncreaseToHealth),
            new EarthDragon(_earthDragonSkillCooldown, _earthDragonIcon, _earthDragonIncreaseToHealth),
            new WitchOfChaos(_witchSkillCooldown, _witchIcon, _witchIncreaseToHealth)
        };

        foreach (var enemy in enemies)
            _enemySkillFactory.SetNewSkill(enemy);

        return enemies;
    }
}

enum Enemies
{
    Goblin,
    Orc,
    Yeti,
    OrcChieftain,
    Fenrir,
    Gargoyle,
    SnowQueen,
    EarthDragon,
    WitchOfChaos
}