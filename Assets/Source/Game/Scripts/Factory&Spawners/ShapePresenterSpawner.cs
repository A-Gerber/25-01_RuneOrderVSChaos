using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

internal class ShapePresenterSpawner : Spawner<ShapePresenter>, IReportableOnRelease
{
    [SerializeField] private Transform[] _pointsSpawn;
    [SerializeField] private CubeViewSpawner _cubeViewSpawner;
    [SerializeField] private SmallCubeSpawner _smallCubeSpawner;
    [SerializeField] private ArrowFactory _arrowFactory;
    [SerializeField] private float _gridStep = 0f;
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _mobileShift = 2f;

    private List<CubeView> _currentCubeViews;
    private ISubscribeable _userSkillPerformer;
    private IProcessable _game;
    private IChangeableRuneDisplay _area;
    private int _index = 0;
    private ArrowView _arrowView;

    public event Action<Shape> CreatedShape;
    public event Action<int> ReleasedShape;

    private void OnEnable()
    {
        _cubeViewSpawner.CreatedCubeView += OnGetShape;
    }

    private void OnDisable()
    {
        _cubeViewSpawner.CreatedCubeView -= OnGetShape;
    }

    internal void Initialize(IProcessable game, ISubscribeable userSkillPerformer, IChangeableRuneDisplay area)
    {
        _game = game ?? throw new InvalidOperationException("game is null");
        _userSkillPerformer = userSkillPerformer ?? throw new InvalidOperationException("userSkillPerformer is null");
        _area = area ?? throw new InvalidOperationException("area is null");
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

        if (YG2.envir.isDesktop)
        {
            Shape shape = new(@object.transform, new MoverBehindCursor(_gridStep, _speed), new ShapeShifter());
            @object.Initialize(shape, new ShapeRotater(@object.transform), _area);
        }
        else
        {
            Shape shape = new(@object.transform, new MobileMoverBehindCursor(_gridStep, _speed), new MobileShapeShifter(_mobileShift));
            @object.Initialize(shape, new ShapeRotater(@object.transform), _area);
        }

        return @object;
    }

    protected override void OnRelease(ShapePresenter shape)
    {
        if (shape == null)
            throw new InvalidOperationException("shape is null");

        base.OnRelease(shape);

        if (shape.IsRestart == false)
        {
            ReleasedShape?.Invoke(shape.CubeCount);
            _game.ProcessStepOverTime();
        }

        shape.RemoveCubes();
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
                _arrowView.transform.localPosition = new Vector3(0f, 1f, 0f);
            }
        }

        _currentCubeViews = cubeViews;
        Get();
    }
}
