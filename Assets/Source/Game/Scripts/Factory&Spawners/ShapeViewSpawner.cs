using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class ShapeViewSpawner : Spawner<ShapeView>
    {
        [SerializeField] private Transform[] _pointsSpawn;
        [SerializeField] private CubeViewSpawner _cubeViewSpawner;

        private readonly ShapeModelFactory _modelFactory = new();
        private List<CubeView> _currentCubeViews;
        private IProcessable _game;
        private int _index = 0;

        internal event Action<ShapeModel> CreatedShape;

        private void OnEnable()
        {
            _cubeViewSpawner.CreatedCubeView += OnGetShape;
        }

        private void OnDisable()
        {
            _cubeViewSpawner.CreatedCubeView -= OnGetShape;
        }

        internal void Initialize(IProcessable game)
        {
            _game = game ?? throw new InvalidOperationException("game is null");
        }

        internal void CreateShape(ICubeConfigurator configurator)
        {
            if (configurator == null)
                throw new InvalidOperationException("configurator is null");

            _cubeViewSpawner.CreateCubes(configurator.CreateConfiguration());
        }

        protected override ShapeView Create()
        {
            ShapeView @object = Instantiate(Prefab);
            @object.Initialize(_modelFactory.Create(@object.transform, @object.GetMover(), @object.DurationOfReturn));

            return @object;
        }

        protected override void OnRelease(ShapeView shape)
        {
            if (shape == null)
                throw new InvalidOperationException("shape is null");

            base.OnRelease(shape);

            shape.RemoveCubes();

            if (shape.IsRestart == false)
                _game.ProcessStep();

            shape.Released -= Release;
        }

        protected override void OnGet(ShapeView shape)
        {
            if (shape == null)
                throw new InvalidOperationException("shape is null");

            base.OnGet(shape);

            shape.transform.position = _pointsSpawn[_index].position;
            shape.SetPosition(_pointsSpawn[_index].position);
            shape.TakeCubes(_currentCubeViews);
            shape.Reduce();
            _index = ++_index % _pointsSpawn.Length;

            CreatedShape?.Invoke(shape.GetShapeModel());
            shape.Released += Release;
        }

        private void OnGetShape(List<CubeView> cubeViews)
        {
            _currentCubeViews = cubeViews;
            Get();
        }
    }
}