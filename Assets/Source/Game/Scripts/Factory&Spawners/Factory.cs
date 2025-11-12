using UnityEngine;

namespace RuneOrderVSChaos
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private AreaFactory _areaFactory;
        [SerializeField] private ShapeViewSpawner _shapeViewSpawner;
        [SerializeField] private EnemiesFactory _enemiesFactory;
        [SerializeField] private ProjectileSpawner _projectileSpawner;
        [SerializeField] private AttackerFactory _attackerFactory;

        internal GameModel Create()
        {
            AreaModel areaModel = _areaFactory.Create();
            _projectileSpawner.Initialize(_enemiesFactory.GetSpawnPosition());
            AttackerModel attacker = _attackerFactory.Create();

            GameModel gameModel = new(areaModel, attacker, _shapeViewSpawner, _enemiesFactory, _projectileSpawner);
            _shapeViewSpawner.Initialize(gameModel);

            return gameModel;
        }
    }
}