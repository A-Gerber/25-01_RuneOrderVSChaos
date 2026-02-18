using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

internal class Factory : MonoBehaviour, IFactoryData
{
    [SerializeField] private AreaViewFactory _areaFactory;
    [SerializeField] private ShapePresenterSpawner _shapePresenterSpawner;
    [SerializeField] private EnemiesFactory _enemiesFactory;
    [SerializeField] private ProjectileSpawner _projectileSpawner;
    [SerializeField] private AttackerFactory _attackerFactory;
    [SerializeField] private UserSkillFactory _skillFactory;
    [SerializeField] private StalactiteViewSpawner _stalactiteViewSpawner;
    [SerializeField] private GroundImpactEffectSpawner _groundImpactEffectSpawner;
    [SerializeField] private FreezingEffectSpawner _freezingEffectSpawner;
    [SerializeField] private EffectConfettiSpawner _effectConfettiSpawner;
    [SerializeField] private PlayerInputController _controller;
    [SerializeField] private float _flightAltitude = 2f;

    private UserSkillHandler _userSkillHandler;
    private ShapeModelFactory _shapeModelFactory;

    public ShapePresenterSpawner ShapePresenterSpawner => _shapePresenterSpawner;
    public EnemiesFactory EnemiesFactory => _enemiesFactory;
    public ICreateableBullets ProjectileSpawner => _projectileSpawner;
    public PlayerInputController PlayerInputController => _controller;
    public IChangeableLevel UserSkillHandler => _userSkillHandler;

    internal GameModel CreateGameModel()
    {
        AreaModel areaModel = _areaFactory.Create();
        _projectileSpawner.Initialize(_enemiesFactory.GetEnemyPosition());
        AttackerModel attacker = _attackerFactory.Create();
        UserSkillPerformer userSkillPerformer = _skillFactory.CreateUserSkillPerformer();
        _userSkillHandler = _skillFactory.CreateUserSkillHandler(attacker);
        userSkillPerformer.Initialize(areaModel, _shapePresenterSpawner.GetCubesSpawner());
        EnemySkillPerfomer enemySkillPerfomer = _enemiesFactory.Create(areaModel, _stalactiteViewSpawner, _groundImpactEffectSpawner, _freezingEffectSpawner);

        GameModel gameModel = new(this, areaModel, attacker, userSkillPerformer, enemySkillPerfomer);

        _shapeModelFactory = new(UserUtilities.CameraHeight - _flightAltitude);
        _shapePresenterSpawner.Initialize(gameModel, _shapeModelFactory, userSkillPerformer);
        _enemiesFactory.SetGameForEnemyPresenter(gameModel);

        return gameModel;
    }

    internal ISkillCardDiscoverer GetSkillCardDiscoverer()
    {
        return _skillFactory.SkillCardDiscoverer;
    }

    internal void InitializeUserSkillHandler(ISettingableSkillButton gameView)
    {
        _userSkillHandler.Initialize(gameView);
    }

    internal void InitializeEffectConfettiSpawner(IWinable game)
    {
        _effectConfettiSpawner.Initialize(game);
    }
}