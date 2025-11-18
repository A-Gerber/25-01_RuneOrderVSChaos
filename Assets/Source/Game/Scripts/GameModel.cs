using System;
using System.Collections.Generic;
using UnityEngine;

internal class GameModel : IProcessable
{
    private const int ShapeCountForCreate = 3;
    private const int StartLevel = 1;
    private const int StartSkillCount = 5;

    private readonly ShapeModel[] _shapeModels = new ShapeModel[ShapeCountForCreate];

    private ShapeViewSpawner _shapeViewSpawner;
    private EnemiesFactory _enemiesFactory;
    private ICreateableBullets _projectileSpawner;
    private AreaModel _area;
    private IEnemy _enemy;
    private AttackerModel _attacker;
    private PlayerInputController _controller;
    private ConfigurationGenerator _configurationGenerator = new(StartLevel);
    private SkillUser _skillUser;

    private int _index = 0;
    private int _skillCount = 0;
    private int _level;

    internal GameModel(AreaModel area, AttackerModel attacker, ShapeViewSpawner shapeViewSpawner, EnemiesFactory enemiesFactory, ICreateableBullets projectileSpawner, PlayerInputController controller, SkillUser skillUser)
    {
        _shapeViewSpawner = shapeViewSpawner ?? throw new InvalidOperationException("shapeViewSpawner is null");
        _enemiesFactory = enemiesFactory ?? throw new InvalidOperationException("enemiesFactory is null");
        _projectileSpawner = projectileSpawner ?? throw new InvalidOperationException("projectileSpawner is null");
        _area = area ?? throw new InvalidOperationException("area is null");
        _attacker = attacker ?? throw new InvalidOperationException("attacker is null");
        _controller = controller ?? throw new InvalidOperationException("attacker is null");
        _skillUser = skillUser ?? throw new InvalidOperationException("skillUser is null");

        _area.Initialize(_shapeModels);

        _shapeViewSpawner.CreatedShape += OnCreateShapeView;  // Подумать как отписаться
        _controller.SkillButtonClicked += OnUseSkill;  // Подумать как отписаться
        Debug.Log("Подумать как отписаться");
    }

    internal event Action Waited; // Как лучше сделать замер времени
    internal event Action GameOvered;
    internal event Action GameWined;
    internal event Action<int> LevelUpped;
    internal event Action<int> SkillCountChanged;

    public void ProcessStepOverTime()
    {
        Waited?.Invoke();
        Debug.Log("Как лучше сделать замер времени");
    }

    internal void NewGame()
    {
        _level = StartLevel;
        _skillCount = StartSkillCount;
        LevelUpped?.Invoke(_level);
        SkillCountChanged?.Invoke(_skillCount);
        _index = ShapeCountForCreate;
        _configurationGenerator.StartLevel();
        CreateShapes();
        CreateEnemy();
    }

    internal void Restart()
    {
        _index = ShapeCountForCreate;
        _configurationGenerator.StartLevel();
        _enemy.Restart();
        _area.Restart();
        CreateShapes();
    }

    internal void GoToNextLevel()
    {
        _level++;
        LevelUpped?.Invoke(_level);
        _index = ShapeCountForCreate;
        _configurationGenerator.StartLevel();
        CreateEnemy();
        _area.Restart();
        CreateShapes();
    }

    internal void ProcessStep()
    {
        if (_area.TryFindTargetCellsByLines())      
            Attack();
        
        CreateShapes();

        if (_enemy.IsAlive && _area.IsLostGame() && _skillCount <= 0)        
            GameOvered?.Invoke();       
    }

    internal void PressSkillButton()
    {
        _skillUser.PressButton();
    }

    private void Attack()
    {
        _projectileSpawner.CreateBullets(_area.GetPositionTargetCells());
        _attacker.Attack(_area.CountTargetCells);
        _area.ReleaseTargetCubes();

        if (_enemy.IsAlive == false)
            GameWined?.Invoke();
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
        if (_skillUser.TryGetSkillCoordinates(out List<LocalPosition> coordinates, _skillCount))
        {
            _skillUser.UseSkill();
            _skillCount--;
            SkillCountChanged?.Invoke(_skillCount);

            if (_area.TryFindTargetCellsByCoordinates(coordinates))
            {
                Attack();
            }
        }

        if (_enemy.IsAlive && _area.IsLostGame() && _skillCount <= 0)
            GameOvered?.Invoke();
    }
}
