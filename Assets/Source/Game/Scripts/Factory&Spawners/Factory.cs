using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private AreaViewFactory _areaFactory;
    [SerializeField] private ShapeViewSpawner _shapeViewSpawner;
    [SerializeField] private EnemiesFactory _enemiesFactory;
    [SerializeField] private ProjectileSpawner _projectileSpawner;
    [SerializeField] private AttackerFactory _attackerFactory;
    [SerializeField] private SkillFactory _skillFactory;
    [SerializeField] private PlayerInputController _controller;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _flightAltitude = 2f;

    internal GameModel CreateGameModel()
    {
        AreaModel areaModel = _areaFactory.Create();
        _projectileSpawner.Initialize(_enemiesFactory.GetSpawnPosition());
        AttackerModel attacker = _attackerFactory.Create();
        Skill skill = new Skill();
        SkillUser skillUser = _skillFactory.Create(_areaFactory.MinBorderArea, _areaFactory.MaxBorderArea, _camera.transform.position.y);

        GameModel gameModel = new GameModel (areaModel, attacker, _shapeViewSpawner, _enemiesFactory, _projectileSpawner, _controller, skillUser);
        _shapeViewSpawner.Initialize(gameModel, _camera.transform.position.y - _flightAltitude);

        return gameModel;
    }
}