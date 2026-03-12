using UnityEngine;

internal class LevelInstaller : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private ConstantsInstaller _constantsInstaller;
    [SerializeField] private Factory _factory;
    [SerializeField] private MenuView _menu;
    [SerializeField] private GameView _gameView;
    [SerializeField] private CellView _cellViewPrefab;
    [SerializeField] private CubeView _runePrefab;

    private void Awake()
    {
        _constantsInstaller.SetParameters(_camera.transform.position.y, _cellViewPrefab.transform.localScale.x, _runePrefab.GetCubeSize());
        _constantsInstaller.SetConstants();
             
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
