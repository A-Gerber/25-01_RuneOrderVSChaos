using System;
using System.Collections.Generic;
using UnityEngine;

internal class ShapePresenterSpawner : Spawner<ShapePresenter>
{
    [SerializeField] private Transform[] _pointsSpawn;
    [SerializeField] private CubeViewSpawner _cubeViewSpawner;
    [SerializeField] private SmallCubeSpawner _smallCubeSpawner;
    [SerializeField] private ArrowFactory _arrowFactory;
    [SerializeField] private float _speed = 20f;

    private ShapeModelFactory _modelFactory;
    private List<CubeView> _currentCubeViews;
    private ISubscribeable _userSkillPerformer;
    private IProcessable _game;
    private int _index = 0;
    private ArrowView _arrowView;

    internal event Action<Shape> CreatedShape;

    private void OnEnable()
    {
        _cubeViewSpawner.CreatedCubeView += OnGetShape;
    }

    private void OnDisable()
    {
        _cubeViewSpawner.CreatedCubeView -= OnGetShape;
    }

    internal void Initialize(IProcessable game, ShapeModelFactory modelFactory, ISubscribeable userSkillPerformer)
    {
        _game = game ?? throw new InvalidOperationException("game is null");
        _modelFactory = modelFactory ?? throw new InvalidOperationException("modelFactory is null");
        _userSkillPerformer = userSkillPerformer ?? throw new InvalidOperationException("userSkillPerformer is null");
    }

    internal void CreateShape(ICubeConfigurator configurator)
    {
        if (configurator == null)
            throw new InvalidOperationException("configurator is null");

        List<LocalPosition> coordinates = configurator.CreateConfiguration();
        _arrowView = _arrowFactory.Create(coordinates, _smallCubeSpawner);
        _userSkillPerformer.SubscribeToArrow(_arrowView.GetArrow());
        _cubeViewSpawner.CreateCubesForShape(coordinates);
    }

    internal ICreateableCubesForArea GetCubesSpawner()
    {
        return _cubeViewSpawner;
    }

    protected override ShapePresenter Create()
    {
        ShapePresenter @object = Instantiate(Prefab);
        @object.Initialize(_modelFactory.Create(@object.transform, @object.DurationOfReturn, _speed),new ShapeRotater(@object.transform));

        return @object;
    }

    protected override void OnRelease(ShapePresenter shape)
    {
        if (shape == null)
            throw new InvalidOperationException("shape is null");

        base.OnRelease(shape);

        shape.RemoveCubes();

        if (shape.IsRestart == false)
            _game.ProcessStepOverTime();

        shape.Released -= Release;
    }

    protected override void OnGet(ShapePresenter shape)
    {
        if (shape == null)
            throw new InvalidOperationException("shape is null");

        base.OnGet(shape);

        shape.transform.position = _pointsSpawn[_index].position;
        shape.SetPosition(_pointsSpawn[_index].position);
        shape.Take(_currentCubeViews, _arrowView.GetArrow());
        shape.Reduce();
        _index = ++_index % _pointsSpawn.Length;

        CreatedShape?.Invoke(shape.GetShapeModel());
        shape.Released += Release;
    }

    private void OnGetShape(List<CubeView> cubeViews)
    {
        foreach (var cubeView in cubeViews)
        {
            if (UserUtilities.IsEqualVector3(cubeView.LocalPosition, _arrowView.Position))
            {
                _arrowView.transform.SetParent(cubeView.transform);
                _arrowView.transform.localPosition = new Vector3(0f,1f,0f);
            }
        }

        _currentCubeViews = cubeViews;
        Get();
    }
}
