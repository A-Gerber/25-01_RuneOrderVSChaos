using UnityEngine;
using YG;

internal class LevelInstaller : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private ConstantsInstaller _constantsInstaller;
    [SerializeField] private LanguageHandler _languageHandler;
    [SerializeField] private Factory _factory;
    [SerializeField] private TaskFactory _taskFactory;
    [SerializeField] private MenuView _menu;
    [SerializeField] private GameView _gameView;
    [SerializeField] private Saver _saver;
    [SerializeField] private CellView _cellViewPrefab;
    [SerializeField] private CubeView _runePrefab;
    [SerializeField] private int _startLevel = 1;
    [SerializeField] private int _startManaCount = 20;
    [SerializeField] private int _startGameScore = 0;

    private GameModel _gameModel;
    private TaskHandler _taskHandler;

    private void Awake()
    {
        _constantsInstaller.SetParameters(_camera.transform.position.y, _cellViewPrefab.transform.localScale.x, _runePrefab.GetCubeSize());
        _constantsInstaller.SetConstants(_startLevel);
        _constantsInstaller.SetLanguage(YG2.lang);

        _gameModel = _factory.CreateGameModel();
        _gameView.Initialize(_gameModel, _menu);
        _factory.InitializeUserSkillHandler(_gameView);
        _factory.InitializeEffectConfettiSpawner(_gameModel);

        _factory.CreateYandexEntities(_menu);

        _saver.SetStartData(new GameSavedData(_startLevel, _startManaCount, _startGameScore), new SkillsSavedData(_factory.GetNameOfActivatedSkills()));
        _menu.Initialize(_factory.GetEntityDataForMenu(_saver));
        _languageHandler.Initialize(_factory.GetSkillCardViews());

        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Start()
    {
        //_gameView.NewGame();
        _menu.OpenMenu();
    }

    private void OnStartGame()
    {        
        if (_gameModel.CurrentLevel == _startLevel)
        {
            if(_taskHandler != null)
                _taskHandler.CloseTutorial();

            _taskHandler = _taskFactory.Create();
        }
    }

    private void Subscribe()
    {
        if (_gameModel != null)
        {
            _gameModel.StartedGame += OnStartGame;
        }
    }

    private void Unsubscribe()
    {
        if (_gameModel != null)
        {
            _gameModel.StartedGame -= OnStartGame;
        }
    }
}
