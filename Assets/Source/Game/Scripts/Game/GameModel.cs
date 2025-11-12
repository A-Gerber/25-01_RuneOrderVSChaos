using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class GameModel : IProcessable
    {
        private const int ShapeCountForCreate = 3;
        private const int StartLevel = 1;

        private readonly ShapeModel[] _shapeModels = new ShapeModel[ShapeCountForCreate];

        private ShapeViewSpawner _shapeViewSpawner;
        private EnemiesFactory _enemiesFactory;
        private ICreateableBullets _projectileSpawner;
        private AreaModel _area;
        private IEnemy _enemy;
        private AttackerModel _attacker;
        private readonly StartCubeConfigurator _startConfigurator = new();
        private readonly SimpleCubeConfigurator _simpleCubeConfigurator = new();
        private readonly MiddleCubeConfigurator _middleCubeConfigurator = new();
        private readonly HardCubeConfigurator _hardCubeConfigurator = new();

        private int _index = 0;

        internal GameModel(AreaModel area, AttackerModel attacker, ShapeViewSpawner shapeViewSpawner, EnemiesFactory enemiesFactory, ICreateableBullets projectileSpawner)
        {
            _shapeViewSpawner = shapeViewSpawner ?? throw new InvalidOperationException("shapeViewSpawner is null");
            _enemiesFactory = enemiesFactory ?? throw new InvalidOperationException("enemiesFactory is null");
            _projectileSpawner = projectileSpawner ?? throw new InvalidOperationException("projectileSpawner is null");
            _area = area ?? throw new InvalidOperationException("area is null");
            _attacker = attacker ?? throw new InvalidOperationException("attacker is null");

            _area.Initialize(_shapeModels);

            _shapeViewSpawner.CreatedShape += OnCreateShapeView;  // Подумать как отписаться
            Debug.Log("Подумать как отписаться");
        }

        internal event Action Waited;
        internal event Action GameOvered;
        internal event Action GameWined;

        internal int Level { get; private set; }

        public void Win()
        {

        }

        public void ProcessStep()
        {
            Waited?.Invoke();
        }

        internal void NewGame()
        {
            Level = StartLevel;
            CreateEnemy();

            for (int i = 0; i < ShapeCountForCreate; i++)
                _shapeViewSpawner.CreateShape(_simpleCubeConfigurator);
            //_shapeViewSpawner.CreateShape(_startConfigurator);
        }

        internal void Restart()
        {
            _index = ShapeCountForCreate;
            _enemy.Restart();
            _area.Restart();
            CreateShapes();
        }

        internal void Attack()
        {
            if (_area.TryGetCountTargetCells(out int countCells))
            {
                _projectileSpawner.CreateBullets(_area.GetPositionTargetCells());
                _area.ReleaseCubesInLine();
                _attacker.Attack(countCells);
            }

            CreateShapes();

            if (_enemy.IsAlive == false)
            {
                Level++;
                _index = ShapeCountForCreate;
                CreateEnemy();
                _area.Restart();
                CreateShapes();

                GameWined?.Invoke();
                return;
            }

            if (_area.IsLostGame())
            {
                GameOvered?.Invoke();
            }
        }

        private void CreateShapes()
        {
            if (++_index < ShapeCountForCreate)
                return;

            for (int i = 0; i < ShapeCountForCreate; i++)
            {
                _shapeViewSpawner.CreateShape(_simpleCubeConfigurator);
                //_shapeViewSpawner.CreateShape(_hardCubeConfigurator);
            }

            _index = 0;
        }

        private void CreateEnemy()
        {
            _enemy = _enemiesFactory.Create(Level);
            _attacker.SetEnemy(_enemy);
        }

        private void OnCreateShapeView(ShapeModel shapeModel)
        {
            _area.TakeShapeModel(shapeModel);
        }
    }
}