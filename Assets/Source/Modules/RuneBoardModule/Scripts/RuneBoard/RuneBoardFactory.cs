using UnityEngine;

public class RuneBoardFactory : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasContainer;
    [SerializeField] private Transform _runeBoardContainer;
    [SerializeField] private CellFactory _cellFactory;    
    [SerializeField] private IncreasedDamageScreen _increasedDamageScreen;    
    [SerializeField] private float _heightOfForceImpact = -1f;
    [SerializeField] private float _forceImpact = 5f;

    [Header("Factories&Spawners")]
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private AttackerFactory _attackerFactory;
    [SerializeField] private ShapePresenterSpawner _shapePresenterSpawner;
    [SerializeField] private ProjectileSpawner _projectileSpawner;
    [SerializeField] private StalactiteViewSpawner _stalactiteViewSpawner;
    [SerializeField] private GroundImpactEffectSpawner _groundImpactEffectSpawner;
    [SerializeField] private FreezingEffectSpawner _freezingEffectSpawner;

    [Header("Prefabs")]
    [SerializeField] private RuneBoardPresenter _runeBoardPrefab;
    [SerializeField] private ConfigurationGenerator _configurationGeneratorPrefab;
    [SerializeField] private RuneBoardView _runeBoardViewPrefab;

    private RuneBoardPresenter _runeBoardPresenter;
    private EnemySkillPerfomer _enemySkillPerfomer;
    private Attacker _attacker;
    private PlayField _playField;

    public IPlayFieldSkillContactable PlayField => _playField;
    public IShapeSpawnerSkillContactable ShapePresenterSpawner => _shapePresenterSpawner;
    public IChangeableLanguage EnemyPresenter => _enemyFactory.EnemyPresenter;
    public IAttackerSkillContactable Attacker => _attacker;
    public IAttackerPresenter AttackerPresenter => _attackerFactory.AttackerPresenter;
    public IncreasedDamageScreen IncreasedDamageScreen => _increasedDamageScreen;

    public RuneBoard Create(IOpenable menu, IRuneBoardSkillMediator mediator)
    {
        _cellFactory.Create();

        RuneBoard runeBoard = new();
        RuneBoardView runeBoardView = Instantiate(_runeBoardViewPrefab, _canvasContainer);
        _enemyFactory.SetPresenters(runeBoardView.EnemyPerformers);

        RuneDisplayer runeDisplayer = new(_cellFactory.GetListCells());
        ShapePlatform shapePlatform = new(new Shape[Constants.ShapeCountForCreate], _shapePresenterSpawner, Instantiate(_configurationGeneratorPrefab, _runeBoardContainer));
        _playField = new(_cellFactory.GetCells(), runeDisplayer, shapePlatform, new Pusher(_heightOfForceImpact, _forceImpact));
        _attacker = _attackerFactory.Create(_enemyFactory, _projectileSpawner, _increasedDamageScreen);

        EnemyEffectSpawner enemyEffectSpawners = new(_stalactiteViewSpawner, _groundImpactEffectSpawner, _freezingEffectSpawner);
        _enemySkillPerfomer = _enemyFactory.Create(_playField, shapePlatform, enemyEffectSpawners);

        runeBoard.Initialize(CreateEntityData(_playField, _attacker, mediator));
        _runeBoardPresenter = Instantiate(_runeBoardPrefab, _runeBoardContainer);

        _projectileSpawner.Initialize(_enemyFactory.EnemyPosition);
        _shapePresenterSpawner.Initialize(runeDisplayer, _runeBoardPresenter, _playField);
        runeBoardView.Initialize(menu);
        _runeBoardPresenter.Initialize(runeBoard, runeBoardView);

        return runeBoard;
    }

    public void Set(bool isDesktop)
    {
        _shapePresenterSpawner.Set(isDesktop);
    }

    private EntityDataForRuneBoard CreateEntityData(PlayField playField, Attacker attacker, IRuneBoardSkillMediator mediator)
    {
        EntityDataForRuneBoard entityDataForRuneBoard = new ();
        entityDataForRuneBoard.TakeRuneBoardEntities(playField, attacker);
        entityDataForRuneBoard.Take(_enemySkillPerfomer, mediator);

        return entityDataForRuneBoard;
    }
}
