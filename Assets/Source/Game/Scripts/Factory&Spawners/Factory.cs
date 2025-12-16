using UnityEngine;
using UnityEngine.InputSystem.XR;

internal class Factory : MonoBehaviour, IFactoryData
{
    [SerializeField] private AreaViewFactory _areaFactory;
    [SerializeField] private ShapeViewSpawner _shapeViewSpawner;
    [SerializeField] private EnemiesFactory _enemiesFactory;
    [SerializeField] private ProjectileSpawner _projectileSpawner;
    [SerializeField] private AttackerFactory _attackerFactory;
    [SerializeField] private UserSkillFactory _skillFactory;
    [SerializeField] private PlayerInputController _controller;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _flightAltitude = 2f;
    [SerializeField] private int _areaSize = 8;

    private UserSkillHandler _userSkillHandler;

    public ShapeViewSpawner ShapeViewSpawner => _shapeViewSpawner;
    public EnemiesFactory EnemiesFactory => _enemiesFactory;
    public ICreateableBullets ProjectileSpawner => _projectileSpawner;
    public PlayerInputController PlayerInputController => _controller;
    public IChangeableLevel UserSkillHandler => _userSkillHandler;

    internal GameModel CreateGameModel()
    {
        AreaModel areaModel = _areaFactory.Create(_areaSize);
        _projectileSpawner.Initialize(_enemiesFactory.GetEnemyPosition());
        AttackerModel attacker = _attackerFactory.Create(_areaSize);
        UserSkillPerformer userSkillPerformer = _skillFactory.CreateUserSkillPerformer(_areaFactory.MinBorderArea, _areaFactory.MaxBorderArea, _camera.transform.position.y);
        _userSkillHandler = _skillFactory.CreateUserSkillHandler(attacker);
        userSkillPerformer.Initialize(areaModel, _shapeViewSpawner.GetCubesSpawner());

        GameModel gameModel = new(this, areaModel, attacker, userSkillPerformer);
        _shapeViewSpawner.Initialize(gameModel, _camera.transform.position.y - _flightAltitude);

        return gameModel;
    }

    internal void InitializeUserSkillHandler(ISettingableSkillButton gameView)
    {
        _userSkillHandler.Initialize(gameView);
    }
}