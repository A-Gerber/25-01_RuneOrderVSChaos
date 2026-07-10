using System;
using System.Collections.Generic;
using UnityEngine;

internal class ShapePresenterSpawner : Spawner<ShapePresenter>, IShapeSpawnerSkillContactable
{
    [SerializeField] private Transform[] _pointsSpawn;
    [SerializeField] private CubePresenterSpawner _cubePresenterSpawner;
    [SerializeField] private SmallCubeSpawner _smallCubeSpawner;
    [SerializeField] private ArrowFactory _arrowFactory;
    [SerializeField] private float _gridStep = 0f;
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _mobileShift = 2f;

    private List<CubePresenter> _currentCubePresenters;
    private IDisplayChangeable _runeDisplayer;
    private IProcessableStep _runeBoardPresenter;
    private ArrowPresenter _arrowPresenter;
    private int _index = 0;
    private bool _isDesktop;

    public event Action<int> ReleasedShape;
    internal event Action<Shape> Geted;

    private void OnEnable()
    {
        _cubePresenterSpawner.CreatedCubePresenters += OnGetShape;
    }

    private void OnDisable()
    {
        _cubePresenterSpawner.CreatedCubePresenters -= OnGetShape;
    }

    public void CreateCubesUsingSkill(List<LocalPosition> coordinates)
    {
        _cubePresenterSpawner.CreateCubesForArea(coordinates);
    }

    internal void Set(bool isDesktop)
    {
        _isDesktop = isDesktop;
    }

    internal void Initialize(IDisplayChangeable runeDisplayer, IProcessableStep runeBoardPresenter, ICellGetable playField)
    {
        _runeDisplayer = runeDisplayer ?? throw new ArgumentNullException("runeDisplayer is null", nameof(runeDisplayer));
        _runeBoardPresenter = runeBoardPresenter ?? throw new ArgumentNullException("runeBoardPresenter is null", nameof(runeBoardPresenter));
        _cubePresenterSpawner.Initialize(playField);
    }

    internal void CreateShape(ICubeConfigurator configurator)
    {
        List<LocalPosition> coordinates = configurator.CreateConfiguration();
        _arrowPresenter = _arrowFactory.Create(coordinates, _smallCubeSpawner);
        _cubePresenterSpawner.CreateCubesForShape(coordinates);
    }

    protected override ShapePresenter Create()
    {
        ShapePresenter @object = Instantiate(Prefab);

        if (_isDesktop)
        {
            Shape shape = new(@object.transform, new ShapeShifter(), new MoverBehindCursor(_gridStep, _speed), _runeDisplayer);
            @object.Initialize(shape, new ShapeRotator(@object.transform));
        }
        else
        {
            Shape shape = new(@object.transform, new MobileShapeShifter(_mobileShift), new MobileMoverBehindCursor(_gridStep, _speed), _runeDisplayer);
            @object.Initialize(shape, new ShapeRotator(@object.transform));
        }

        return @object;
    }

    protected override void OnRelease(ShapePresenter shapePresenter)
    {
        if (shapePresenter == null)
            throw new ArgumentNullException("cell is null", nameof(shapePresenter));

        base.OnRelease(shapePresenter);

        if (shapePresenter.IsRestart == false)
        {
            ReleasedShape?.Invoke(shapePresenter.CubeCount);
            _runeBoardPresenter.ProcessStep();
        }

        shapePresenter.Released -= Release;
    }

    protected override void OnGet(ShapePresenter shapePresenter)
    {
        if (shapePresenter == null)
            throw new ArgumentNullException("cell is null", nameof(shapePresenter));

        base.OnGet(shapePresenter);

        Ñonfigure(shapePresenter);
        Geted?.Invoke(shapePresenter.GetShapeModel());

        _index = ++_index % _pointsSpawn.Length;

        shapePresenter.Released += Release;
    }

    private void Ñonfigure(ShapePresenter shapePresenter)
    {
        List<Cube> cubes = new();

        foreach (var cubePresenter in _currentCubePresenters)
        {
            cubePresenter.transform.SetParent(shapePresenter.CubeContainer);
            LocalPosition localPosition = cubePresenter.CubeModel.LocalPosition;
            cubePresenter.transform.localPosition = new Vector3(localPosition.X, 0, localPosition.Z);
            cubes.Add(cubePresenter.CubeModel);
        }

        shapePresenter.SetStartParametrs(_pointsSpawn[_index].position);
        shapePresenter.Take(cubes, _arrowPresenter.GetArrow());

        _arrowPresenter.GetArrow().Activating += OnArrowActivate;
    }

    private void OnGetShape(List<CubePresenter> cubePresenters)
    {
        if (cubePresenters == null)
            throw new InvalidOperationException("cubePresenters is null");

        if (cubePresenters.Count == 0)
            throw new InvalidOperationException("cubePresenters is empty");

        LocalPosition arrowPosition = UserUtilities.TranslateInLocalPosition(_arrowPresenter.Position);

        foreach (var cube in cubePresenters)
        {
            if (UserUtilities.IsEqualPosition(cube.CubeModel.LocalPosition, arrowPosition))
            {
                _arrowPresenter.transform.SetParent(cube.transform);
                _arrowPresenter.transform.localPosition = new Vector3(0f, 1f, 0f);
                break;
            }
        }

        _currentCubePresenters = cubePresenters;
        Get();
    }

    private void OnArrowActivate(Arrow arrow)
    {
        _cubePresenterSpawner.CreateCubesForArea(arrow.CubePositions);
        arrow.Activating -= OnArrowActivate;
    }
}
