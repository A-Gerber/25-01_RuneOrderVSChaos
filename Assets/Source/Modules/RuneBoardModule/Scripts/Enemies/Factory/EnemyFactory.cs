using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal class EnemyFactory : MonoBehaviour, IGetableEnemy
{
    [SerializeField] private EnemySkillFactory _enemySkillFactory;
    [SerializeField] private Transform _enemyPosition;
    [SerializeField] private TextMeshProUGUI _skillDescription;
    [SerializeField] private int _startEnemyHealth = 65;
    [SerializeField] private int _increasePerLevel = 2;
    [SerializeField] private int _divider = 20;
    [SerializeField] private int _powerOfCoefficient = 3;
    [SerializeField] private int _powerMultiplier = 20;

    [Header("EnemyParameters")]
    [SerializeField] private float _goblinSkillCooldown = 90f;
    [SerializeField] private int _goblinIncreaseToHealth = 0;
    [SerializeField] private Sprite _goblinIcon;

    [SerializeField] private float _orcSkillCooldown = 30f;
    [SerializeField] private int _orcIncreaseToHealth = 25;
    [SerializeField] private Sprite _orcIcon;

    [SerializeField] private float _orcChieftainSkillCooldown = 25f;
    [SerializeField] private int _orcChieftainIncreaseToHealth = 30;
    [SerializeField] private Sprite _orcChieftainIcon;

    [SerializeField] private float _yetiSkillCooldown = 7f;
    [SerializeField] private int _yetiIncreaseToHealth = 0;
    [SerializeField] private Sprite _yetiIcon;

    [SerializeField] private float _fenrirSkillCooldown = 5f;
    [SerializeField] private int _fenrirIncreaseToHealth = 20;
    [SerializeField] private Sprite _fenrirIcon;

    [SerializeField] private float _snowQueenSkillCooldown = 10f;
    [SerializeField] private int _snowQueenIncreaseToHealth = 25;
    [SerializeField] private Sprite _snowQueenIcon;

    [SerializeField] private float _gargoyleSkillCooldown = 10f;
    [SerializeField] private int _gargoyleIncreaseToHealth = 10;
    [SerializeField] private Sprite _gargoyleIcon;

    [SerializeField] private float _earthDragonSkillCooldown = 20f;
    [SerializeField] private int _earthDragonIncreaseToHealth = 20;
    [SerializeField] private Sprite _earthDragonIcon;

    [SerializeField] private float _witchSkillCooldown = 12f;
    [SerializeField] private int _witchIncreaseToHealth = 175;
    [SerializeField] private Sprite _witchIcon;

    private EnemySkillPerfomerPresenter _enemySkillPerfomerPresenter;
    private EnemiesGenerator _generator;
    private EnemySkillPerfomer _enemySkillPerfomer;

    public EnemyPresenter EnemyPresenter { get; private set; }
    internal Vector3 EnemyPosition => _enemyPosition.position;

    public IChangeableHealthEnemy GetEnemy(int level)
    {
        IEnemy enemy = _generator.Generate(level);
        int health = CalculateHealth(level, enemy);
        enemy.SetMaxHealth(health);
        EnemyPresenter.SetEnemy(enemy);
        _enemySkillPerfomer.Initialize(enemy);

        return enemy;
    }

    internal void SetPresenters(RectTransform container)
    {
        EnemyPresenter = container.GetComponent<EnemyPresenter>();
        _enemySkillPerfomerPresenter = container.GetComponent<EnemySkillPerfomerPresenter>();
        EnemyPresenter.Initialize(_skillDescription);
    }

    internal EnemySkillPerfomer Create(PlayField playField, IShapeFreezable shapePlatform, EnemyEffectSpawner enemyEffectSpawners)
    {
        enemyEffectSpawners.GroundImpactEffectSpawner.SetStartPosition(EnemyPosition);

        _generator = new EnemiesGenerator(CreateEnemies());
        _enemySkillPerfomer = new EnemySkillPerfomer(shapePlatform, playField, enemyEffectSpawners);
        _enemySkillPerfomerPresenter.Initialize(_enemySkillPerfomer);

        return _enemySkillPerfomer;
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
