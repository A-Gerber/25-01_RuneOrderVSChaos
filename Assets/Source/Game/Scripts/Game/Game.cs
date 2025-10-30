using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class Game : MonoBehaviour, IGame
    {
        private const int ShapeCountForCreate = 3;

        [SerializeField] private AreaFactory _areaFactory;
        [SerializeField] private ShapeViewSpawner _shapeFactory;
        [SerializeField] private int _enemyHealth = 100;

        private int _index = 3;

        private readonly Attacker _attacker = new();
        private AreaModel _area;
        private Enemy _enemy;
        private readonly StartCubeConfigurator _startConfigurator = new();
        private readonly SimpleCubeConfigurator _cubeConfigurator = new();

        private void Awake()
        {
            _area = _areaFactory.Create();
            _area.Initialize(this);
            _shapeFactory.Initialize(this);
            _enemy = new(_enemyHealth);

            CreateStartShapes();
        }

        public void PerformDiagnostics()
        {
            _area.CheckArea();
            _index++;
            CreateShapes();
        }

        public void Attack(int damage)
        {
            _attacker.Attack(_enemy, damage);
        }

        private void CreateShapes()
        {
            if (_index < ShapeCountForCreate)
                return;

            for (int i = 0; i < ShapeCountForCreate; i++)
            {
                _shapeFactory.CreateShape(_cubeConfigurator);
            }

            _index = 0;
        }

        private void CreateStartShapes()
        {
            if (_index < ShapeCountForCreate)
                return;

            for (int i = 0; i < ShapeCountForCreate; i++)
            {
                _shapeFactory.CreateShape(_startConfigurator);
            }

            _index = 0;
        }
    }
}