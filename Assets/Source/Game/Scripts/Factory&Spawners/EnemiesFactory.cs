using System;
using System.Collections.Generic;
using UnityEngine;

internal class EnemiesFactory : MonoBehaviour
{
    [SerializeField] private EnemySkillFactory _enemySkillFactory;
    [SerializeField] private EnemyPresenter _enemyPresenter;
    [SerializeField] private EnemySkillPerfomerView _enemySkillPerfomerView;
    [SerializeField] private int _startEnemyHealth = 60;
    [SerializeField] private int _increasePerLevel = 10;
    [SerializeField] private int _divider = 5;

    [Header("EnemyParameters")]
    [SerializeField] private float _goblinSkillCooldown = 180f;
    [SerializeField] private int _goblinIncreaseToHealth = 0;
    [SerializeField] private Sprite _goblinIcon;

    [SerializeField] private float _orcSkillCooldown = 60f;
    [SerializeField] private int _orcIncreaseToHealth = 50;
    [SerializeField] private Sprite _orcIcon;

    [SerializeField] private float _orcChieftainSkillCooldown = 60f;
    [SerializeField] private int _orcChieftainIncreaseToHealth = 50;
    [SerializeField] private Sprite _orcChieftainIcon;

    [SerializeField] private float _yetiSkillCooldown = 40f;
    [SerializeField] private int _yetiIncreaseToHealth = 30;
    [SerializeField] private Sprite _yetiIcon;

    [SerializeField] private float _fenrirSkillCooldown = 40f;
    [SerializeField] private int _fenrirIncreaseToHealth = 80;
    [SerializeField] private Sprite _fenrirIcon;

    [SerializeField] private float _snowQueenSkillCooldown = 40f;
    [SerializeField] private int _snowQueenIncreaseToHealth = 80;
    [SerializeField] private Sprite _snowQueenIcon;

    [SerializeField] private float _gargoyleSkillCooldown = 40f;
    [SerializeField] private int _gargoyleIncreaseToHealth = 50;
    [SerializeField] private Sprite _gargoyleIcon;

    [SerializeField] private float _earthDragonSkillCooldown = 40f;
    [SerializeField] private int _earthDragonIncreaseToHealth = 50;
    [SerializeField] private Sprite _earthDragonIcon;

    [SerializeField] private float _witchSkillCooldown = 40f;
    [SerializeField] private int _witchIncreaseToHealth = 50;
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

        return _startEnemyHealth + _increasePerLevel * level + coefficient * _increasePerLevel + enemy.IncreaseToHealth;
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
