using System;
using UnityEngine;

internal class GameModel : IProcessable, IGame
{
    private const int ShapeCountForCreate = 3;
    private const int StartLevel = 1;
    private const int StartSkillCount = 1;

    private readonly ShapeModel[] _shapeModels = new ShapeModel[ShapeCountForCreate];

    private readonly ShapeViewSpawner _shapeViewSpawner;
    private readonly EnemiesFactory _enemiesFactory;
    private readonly ICreateableBullets _projectileSpawner;
    private readonly AreaModel _area;
    private readonly AttackerModel _attacker;
    private readonly PlayerInputController _controller;
    private readonly ConfigurationGenerator _configurationGenerator = new(StartLevel);
    private readonly UserSkillPerformer _userPerformer;
    private readonly IChangeableLevel _userSkillHandler;
    private IEnemy _enemy;

    private int _index = 0;
    private int _skillCount;
    private int _level;

    internal GameModel(IFactoryData factory, AreaModel area, AttackerModel attacker, UserSkillPerformer userPerformer)
    {
        if (factory == null)
            throw new InvalidOperationException("shapeViewSpawner is null");
        
        _area = area ?? throw new InvalidOperationException("area is null");
        _attacker = attacker ?? throw new InvalidOperationException("attacker is null");
        _userPerformer = userPerformer ?? throw new InvalidOperationException("userPerformer is null");

        _enemiesFactory = factory.EnemiesFactory;
        _shapeViewSpawner = factory.ShapeViewSpawner;
        _projectileSpawner = factory.ProjectileSpawner;
        _controller = factory.PlayerInputController;
        _userSkillHandler = factory.UserSkillHandler;

        _area.Initialize(_shapeModels);

        _shapeViewSpawner.CreatedShape += OnCreateShapeView;  // Подумать как отписаться
        _controller.UsedSkill += OnUseSkill;  // Подумать как отписаться
        _attacker.SkillPointsAwarded += OnRewardSkillPoints; // Подумать как отписаться
        Debug.Log("Подумать как отписаться");
    }

    public event Action GameOvered;
    public event Action GameWined;
    public event Action Helped;
    public event Action<int> ChangedLevel;
    internal event Action Waited; // Как лучше сделать замер времени
    internal event Action<int> SkillCountChanged;

    public bool IsPlaying { get; private set; } = false;

    public void ProcessStepOverTime()
    {
        Waited?.Invoke();
        Debug.Log("Как лучше сделать замер времени");
    }

    public void NewGame()
    {
        _level = StartLevel;
        ChangedLevel?.Invoke(_level);

        _skillCount = StartSkillCount;
        SkillCountChanged?.Invoke(_skillCount);

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
        CreateShapes();
    }

    public void GoToNextLevel()
    {
        _level++;
        ChangedLevel?.Invoke(_level);

        _skillCount++;
        SkillCountChanged?.Invoke(_skillCount);

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

    internal void ProcessStep()
    {
        if (_area.TryFindTargetCellsByLines())
        {
            _projectileSpawner.CreateBullets(_area.GetPositionTargetCells());
            _attacker.Attack(_area.CountTargetDamage);
            _area.ReleaseTargetCubes();

            if (_enemy.IsAlive == false)
                GameWined?.Invoke();
        }

        CreateShapes();
        IsOverGame();
    }

    internal void PressSkillButton(UserSkill skill)
    {
        if (_skillCount > 0)
            _userPerformer.PressButton(skill);
    }

    private void CreateShapes()
    {
        if (++_index < ShapeCountForCreate)
            return;

        for (int i = 0; i < ShapeCountForCreate; i++)
            _shapeViewSpawner.CreateShape(_configurationGenerator.GenerateConfiguration(_level));

        _index = 0;
    }

    private void CreateEnemy()
    {
        _enemy = _enemiesFactory.Create(_level);
        _attacker.SetEnemy(_enemy);
    }

    private void OnCreateShapeView(ShapeModel shapeModel)
    {
        _area.TakeShapeModel(shapeModel);
    }

    private void OnUseSkill()
    {
        if (_userPerformer.IsPressedButton)
        {
            if (_skillCount > 0 && _userPerformer.TryUseSkill())
            {
                _skillCount--;
                SkillCountChanged?.Invoke(_skillCount);

                _projectileSpawner.CreateBullets(_area.GetPositionTargetCells());
                _attacker.UseSkill(_area.CountTargetDamage);
                _area.ReleaseTargetCubes();

                if (_enemy.IsAlive == false)
                    GameWined?.Invoke();
            }

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
}
