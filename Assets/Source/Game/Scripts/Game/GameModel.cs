using System;
using UnityEngine;

internal class GameModel : IProcessable, IGame, IPerformableAttack
{
    private const int ShapeCountForCreate = 3;

    private readonly Shape[] _shapeModels = new Shape[ShapeCountForCreate];
    private readonly ShapePresenterSpawner _shapePresenterSpawner;
    private readonly EnemiesFactory _enemiesFactory;
    private readonly ICreateableBullets _projectileSpawner;
    private readonly AreaModel _area;
    private readonly AttackerModel _attacker;
    private readonly PlayerInputController _controller;
    private readonly ConfigurationGenerator _configurationGenerator;
    private readonly UserSkillPerformer _userPerformer;
    private readonly EnemySkillPerfomer _enemySkillPerfomer;
    private readonly IChangeableLevel _userSkillHandler;
    private IChangeableHealthEnemy _enemy;

    private readonly int _startLevel;
    private readonly int _startSkillCount;

    private int _index = 0;
    private int _gameScore = 0;
    private int _gameScoreIncrease;
    private int _skillCount;
    private int _level;
    private bool _canAttack;

    internal GameModel(IFactoryData factory, AreaModel area, AttackerModel attacker, UserSkillPerformer userPerformer, EnemySkillPerfomer enemySkillPerfomer)
    {
        if (factory == null)
            throw new InvalidOperationException("shapeViewSpawner is null");

        _area = area ?? throw new InvalidOperationException("area is null");
        _attacker = attacker ?? throw new InvalidOperationException("attacker is null");
        _userPerformer = userPerformer ?? throw new InvalidOperationException("userPerformer is null");
        _enemySkillPerfomer = enemySkillPerfomer ?? throw new InvalidOperationException("enemySkillPerfomer is null");

        _enemiesFactory = factory.EnemiesFactory;
        _shapePresenterSpawner = factory.ShapePresenterSpawner;
        _projectileSpawner = factory.ProjectileSpawner;
        _controller = factory.PlayerInputController;
        _userSkillHandler = factory.UserSkillHandler;

        _area.Initialize(_shapeModels);

        _startLevel = UserUtilities.StartLevel;
        _startSkillCount = UserUtilities.StartSkillCount;
        _configurationGenerator = new(_startLevel);

        _shapePresenterSpawner.CreatedShape += OnCreateShapeView;  // Подумать как отписаться
        _controller.UsedSkill += OnUseSkill;  // Подумать как отписаться
        _attacker.SkillPointsAwarded += OnRewardSkillPoints; // Подумать как отписаться
        _enemySkillPerfomer.PlacedStalactite += IsOverGame; // Подумать как отписаться
        Debug.Log("Подумать как отписаться");
    }

    public event Action GameOvered;
    public event Action Helped;
    public event Action<int> GameWined;
    public event Action<int> ChangedLevel;
    internal event Action<bool> Waited;
    internal event Action<int> SkillCountChanged;
    internal event Action<int> GameScoreChanged;

    public bool IsPlaying { get; private set; } = false;
    public bool CanAttack => _canAttack;
    public int CurrentLevel => _level;

    public void ProcessStepOverTime()
    {
        Waited?.Invoke(false);
    }

    public void NewGame()
    {
        _level = _startLevel;
        ChangedLevel?.Invoke(_level);
        _skillCount = _startSkillCount;
        SkillCountChanged?.Invoke(_skillCount);
        _gameScore = 0;
        GameScoreChanged?.Invoke(_gameScore);

        _index = ShapeCountForCreate;

        _userSkillHandler.ChangeLevel(_level);
        _userSkillHandler.Reset();
        _configurationGenerator.StartLevel();
        CreateEnemy();

        if (IsPlaying)
            _area.Restart();

        CreateShapes();
        IsPlaying = true;
    }

    public void Restart()
    {
        _index = ShapeCountForCreate;
        _configurationGenerator.StartLevel();
        _enemy.Restart();
        _area.Restart();
        _attacker.ResetCounter();
        CreateShapes();
    }

    public void GoToNextLevel()
    {
        _level++;
        ChangedLevel?.Invoke(_level);
        _skillCount += UserUtilities.SkillIncrease;

        SkillCountChanged?.Invoke(_skillCount);
        _gameScore += _gameScoreIncrease;
        GameScoreChanged?.Invoke(_gameScore);
        _attacker.ResetCounter();

        _index = ShapeCountForCreate;
        _userSkillHandler.ChangeLevel(_level);
        _configurationGenerator.StartLevel();
        CreateEnemy();
        _area.Restart();
        CreateShapes();
    }

    public void OnRewardSkillPoints(int numberOfSkillPoints)
    {
        _skillCount += numberOfSkillPoints;
        SkillCountChanged?.Invoke(_skillCount);
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
        if (_skillCount > 0)
            _userPerformer.PressButton(skill);
    }

    internal void UseSkill()
    {
        //_skillCount--;
        SkillCountChanged?.Invoke(_skillCount);

        _projectileSpawner.CreateBullets(_area.GetPositionTargetCells());
        _attacker.UseSkill(_area.CountTargetDamage);
        _area.ReleaseTargetCubes();

        if (_enemy.IsAlive == false)
            Win();

        IsOverGame();
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

    private void OnCreateShapeView(Shape shapeModel)
    {
        _area.TakeShapeModel(shapeModel);
    }

    private void OnUseSkill()
    {
        if (_userPerformer.IsPressedButton)
        {
            if (_skillCount > 0 && _userPerformer.TryUseSkill())            
                Waited?.Invoke(true);         
            else           
                IsOverGame();          
        }
    }

    private void IsOverGame()
    {
        if (_enemy.IsAlive && _area.IsLostGame())
        {
            if (_skillCount <= 0)
                GameOvered?.Invoke();
            else
                Helped?.Invoke();
        }
    }

    private void Win()
    {
        _gameScoreIncrease = _enemy.MaxHealth + _attacker.MaxTotalCombo;
        GameWined?.Invoke(_gameScoreIncrease);
    }
}
