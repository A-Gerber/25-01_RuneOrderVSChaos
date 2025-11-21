using System;
using System.Collections.Generic;
using UnityEngine;

internal class GameModel : IProcessable, IGame
{
    private const int ShapeCountForCreate = 3;
    private const int StartLevel = 1;
    private const int StartSkillCount = 1;

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
    private int _skillCount;
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
        _controller.UsedSkill += OnUseSkill;  // Подумать как отписаться
        _attacker.SkillPointsAwarded += OnRewardSkillPoints; // Подумать как отписаться
        Debug.Log("Подумать как отписаться");
    }

    public event Action GameOvered;
    public event Action GameWined;
    internal event Action Waited; // Как лучше сделать замер времени
    internal event Action<int> LevelUpped;
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
        _skillCount = StartSkillCount;
        LevelUpped?.Invoke(_level);
        SkillCountChanged?.Invoke(_skillCount);
        _index = ShapeCountForCreate;
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
        LevelUpped?.Invoke(_level);
        _index = ShapeCountForCreate;
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
            _attacker.Attack(_area.CountTargetCells);
            _area.ReleaseTargetCubes();

            if (_enemy.IsAlive == false)
                GameWined?.Invoke();
        }

        CreateShapes();

        if (_enemy.IsAlive && _area.IsLostGame() && _skillCount <= 0)
            GameOvered?.Invoke();
    }

    internal void PressSkillButton()
    {
        if (_skillCount > 0)
            _skillUser.PressButton();
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
        if (_skillUser.IsPressedButton)
        {
            Debug.Log("OnUseSkill");
            if (_skillUser.TryGetSkillCoordinates(out List<LocalPosition> coordinates, _skillCount))
            {
                _skillUser.UseSkill();
                _skillCount--;
                SkillCountChanged?.Invoke(_skillCount);

                if (_area.TryFindTargetCellsByCoordinates(coordinates))
                   {
                    _projectileSpawner.CreateBullets(_area.GetPositionTargetCells());
                    _attacker.UseSkill(_area.CountTargetCells);
                    _area.ReleaseTargetCubes();

                    if (_enemy.IsAlive == false)
                        GameWined?.Invoke();
                }
            }

            if (_enemy.IsAlive && _area.IsLostGame() && _skillCount <= 0)
                GameOvered?.Invoke();
        }
    }
}