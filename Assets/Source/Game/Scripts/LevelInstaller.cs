using UnityEngine;

internal class LevelInstaller : MonoBehaviour
{
    private const int OriginByX = 0;
    private const int OriginByZ = 0;
    private const int AreaSize = 8;

    [SerializeField] private Camera _camera;
    [SerializeField] private Factory _factory;
    [SerializeField] private MenuView _menu;
    [SerializeField] private GameView _gameView;
    [SerializeField] private CellView _cellViewPrefab;
    [SerializeField] private CubeView _runePrefab;
    [SerializeField] private int _startLevel = 1;
    [SerializeField] private int _startSkillCount = 1;
    [SerializeField] private int _skillCountIncrease = 1;
    [SerializeField] private int _skillPointsInterval = 5;
    public const int SkillCountIncrease = 1;

    private void Awake()
    {        
        UserUtilities.SetGameParameters(_startLevel, _startSkillCount, _skillCountIncrease, _skillPointsInterval);
        UserUtilities.SetAreaParameters(OriginByX, OriginByZ, AreaSize);
        UserUtilities.SetCameraHeight(_camera.transform.position.y);
        UserUtilities.CalculateAreaBorders(_cellViewPrefab.transform.localScale.x);
        UserUtilities.SetCubeParameters(_runePrefab.GetCubeSize());
             
        GameModel gameModel = _factory.CreateGameModel();
        _gameView.Initialize(gameModel, _menu);
        _menu.Initialize(gameModel, _factory.GetSkillCardDiscoverer());
        _factory.InitializeUserSkillHandler(_gameView);
        _factory.InitializeEffectConfettiSpawner(gameModel);
    }

    private void Start()
    {
        _gameView.NewGame();
        //_menu.OpenMenu();
    }
}