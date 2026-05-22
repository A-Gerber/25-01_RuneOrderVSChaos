using System;
using UnityEngine;

internal class GameModel : IProcessable, IGame, IPerformableAttack, IRewardable
{
    private readonly Shape[] _shapeModels = new Shape[ShapeCountForCreate];
    private readonly ShapePresenterSpawner _shapePresenterSpawner;
    private readonly EnemiesFactory _enemiesFactory;
    private readonly ICreateableBullets _projectileSpawner;
    private readonly AreaModel _area;
    private readonly AttackerModel _attacker;
    private readonly ManaGenerator _manaGenerator;
    private readonly PlayerInputController _controller;
    private readonly UserSkillPerformer _userPerformer;
    private readonly EnemySkillPerfomer _enemySkillPerfomer;

    private const int ShapeCountForCreate = 3;

    private ConfigurationGenerator _configurationGenerator;
    private IChangeableHealthEnemy _enemy;
    private int _index = 0;
    private int _gameScore = 0;
    private int _gameScoreIncrease;
    private int _level;
    private bool _canAttack;

    internal GameModel(EntityDataForGame entityDataForGame)
    {
        if (entityDataForGame == null)
            throw new InvalidOperationException("entityDataForGame is null");

        _area = entityDataForGame.AreaModel ?? throw new InvalidOperationException("AreaModel is null");
        _attacker = entityDataForGame.AttackerModel ?? throw new InvalidOperationException("AttackerModel is null");
        _manaGenerator = entityDataForGame.ManaGenerator ?? throw new InvalidOperationException("ManaGenerator is null");
        _userPerformer = entityDataForGame.UserSkillPerformer ?? throw new InvalidOperationException("UserSkillPerformer is null");
        _enemySkillPerfomer = entityDataForGame.EnemySkillPerfomer ?? throw new InvalidOperationException("EnemySkillPerfomer is null");
        _enemiesFactory = entityDataForGame.EnemiesFactory != null ? entityDataForGame.EnemiesFactory : throw new InvalidOperationException("EnemiesFactory is null");
        _shapePresenterSpawner = entityDataForGame.ShapePresenterSpawner != null ? entityDataForGame.ShapePresenterSpawner : throw new InvalidOperationException("ShapePresenterSpawner is null");
        _projectileSpawner = entityDataForGame.ProjectileSpawner ?? throw new InvalidOperationException("ProjectileSpawner is null");
        _controller = entityDataForGame.PlayerInputController != null ? entityDataForGame.PlayerInputController : throw new InvalidOperationException("PlayerInputController is null");

        _area.Initialize(_shapeModels);  

        _shapePresenterSpawner.CreatedShape += OnCreateShapePresenter;  // Подумать как отписаться
        _shapePresenterSpawner.ReleasedShape += OnReleaseShapePresenter;  // Подумать как отписаться
        _controller.UsedSkill += OnUseSkill;  // Подумать как отписаться
        _attacker.SkillPointsAwarded += RewardForCombo; // Подумать как отписаться
        _enemySkillPerfomer.PlacedStalactite += IsOverGame; // Подумать как отписаться
        Debug.Log("Подумать как отписаться");
    }

    public event Action StartedGame;
    public event Action WentToNextLevel;
    public event Action Helped;
    public event Action DisabledHint;
    public event Action<int> GameOvered;
    public event Action<GameSavedData> GameWined;
    public event Action<bool> Waited;

    public bool IsPlaying { get; private set; } = false;
    public bool CanAttack => _canAttack;
    public int CurrentLevel => _level;
    public int ManaCostPerLevel => _manaGenerator.ManaCostPerLevel;
    public int GameScore => _gameScore;

    public void ProcessStepOverTime()
    {
        Waited?.Invoke(false);
    }

    public void StartGame(GameSavedData data)
    {
        _level = data.Level;
        _manaGenerator.SetStartData(_level, data.ManaCount);
        _gameScore = data.GameScore;
        StartedGame?.Invoke();
        _index = ShapeCountForCreate;
        _configurationGenerator = new(_level);
        _configurationGenerator.ResetTimeCounter();
        
        CreateEnemy();

        if (IsPlaying)
            _area.Restart();

        CreateShapes();
        IsPlaying = true;
    }

    public void Restart()
    {
        _index = ShapeCountForCreate;
        _enemy.Restart();
        _area.Restart();
        _configurationGenerator.ResetTimeCounter();
        _manaGenerator.Restart();
        _attacker.ResetCounter();
        CreateShapes();
    }

    public void GoToNextLevel()
    {
        WentToNextLevel?.Invoke();
        _attacker.ResetCounter();

        _index = ShapeCountForCreate;
        _configurationGenerator.ResetTimeCounter();
        CreateEnemy();
        _area.Restart();
        CreateShapes();
    }

    public void RewardForADV()
    {
        _manaGenerator.RewardForAdvertising();
    }

    public void Attack()
    {
        _canAttack = false;
        _attacker.Attack(_area.CountTargetDamage);

        if (_enemy.IsAlive == false)
            Win();

        CreateShapes();
        IsOverGame();
    }

    internal void ProcessStep()
    {
        if (_area.TryFindTargetCellsByLines())
        {
            _canAttack = true;
            _projectileSpawner.CreateBullets(_area.GetPositionTargetCells());
            _area.ReleaseTargetCubes();
        }
        else
        {
            CreateShapes();
            IsOverGame();
        }
    }

    internal void PressSkillButton(UserSkill skill)
    {
        if (_manaGenerator.CanSpendMana(skill.ManaCost))
            _userPerformer.PressButton(skill);
    }

    internal void UseSkill()
    {
        _manaGenerator.SpendMana(_userPerformer.CurrentManaCost);

        _projectileSpawner.CreateBullets(_area.GetPositionTargetCells());
        _attacker.UseSkill(_area.CountTargetDamage);
        _area.ReleaseTargetCubes();

        if (_enemy.IsAlive == false)
            Win();

        IsOverGame();
    }

    private void OnUseSkill()
    {
        if (_userPerformer.IsPressedButton)
        {
            if (_userPerformer.TryUseSkill())
                Waited?.Invoke(true);
            else
                IsOverGame();
        }
    }

    private void CreateShapes()
    {
        if (++_index < ShapeCountForCreate)
            return;

        for (int i = 0; i < ShapeCountForCreate; i++)
            _shapePresenterSpawner.CreateShape(_configurationGenerator.GetCubeConfigurator(_level));

        _index = 0;
    }

    private void CreateEnemy()
    {
        _enemy = _enemiesFactory.GetEnemy(_level);
        _attacker.SetEnemy(_enemy);
    }

    private void OnCreateShapePresenter(Shape shapeModel)
    {
        _area.TakeShapeModel(shapeModel);
    }

    private void OnReleaseShapePresenter(int cubeCount)
    {
         _manaGenerator.RewardForCubes(cubeCount);
        _configurationGenerator.StartCountdown();
    }

    private void RewardForCombo(int numberOfRewards)
    {
        _manaGenerator.RewardForCombo(numberOfRewards);
    }

    private void IsOverGame()
    {
        if (_enemy.IsAlive && _area.IsLostGame())
        {
            if (_manaGenerator.HaveManaForSkill())
                Helped?.Invoke();
            else
                GameOvered?.Invoke(_manaGenerator.CalculateIncrease());
        }
        else
        {
            DisabledHint?.Invoke();
        }
    }

    private void Win()
    {
        _gameScoreIncrease = _enemy.MaxHealth + _attacker.MaxTotalCombo + _manaGenerator.ManaCount;
        _level++;
        _manaGenerator.RewardForLevel(_level);
        _gameScore += _gameScoreIncrease;

        GameWined?.Invoke(new GameSavedData(_level, _manaGenerator.ManaCount, _gameScore));
    }
}
