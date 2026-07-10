using System.Collections.Generic;
using UnityEngine;

public class GameFactory : MonoBehaviour
{
    private readonly List<IWindowController> _windowsWithPause = new();

    [SerializeField] private RuneBoardFactory _runeBoardFactory;
    [SerializeField] private GeneralUserSkillFactory _skillFactory;
    [SerializeField] private TaskFactory _taskFactory;
    [SerializeField] private Menu _menu;
    [SerializeField] private ModuleLanguageHandler _moduleLanguageHandlerPrefab;
    [SerializeField] private FinalGameHandler _finalGameHandler;
    [SerializeField] private RectTransform _container;
    [SerializeField] private Transform _gameContainer;
    [SerializeField] private Transform _mediatorsPrefab;

    private UserSkillPerformer _userSkillPerformer;
    private RuneBoard _runeBoard;
    private AttackerSkillMediator _attackerSkillMediator;
    private RuneBoardSkillMediator _runeBoardSkillMediator;
    private ShapeSpawnerSkillMediator _shapeSpawnerSkillMediator;
    private PlayFieldSkillMediator _playFieldSkillMediator;

    public ModuleLanguageHandler ModuleLanguageHandler { get; private set; }
    public Menu Menu => _menu;
    public FinalGameHandler FinalGameHandler => _finalGameHandler;
    public UserSkillPerformerPresenter UserSkillPerformerPresenter => _skillFactory.UserSkillPerformerPresenter;
    public IReadOnlyList<IWindowController> WindowsWithPause => _windowsWithPause;
    public IAttackerPresenter AttackerPresenter => _runeBoardFactory.AttackerPresenter;

    public void CreateMediators()
    {
        Transform mediators = Instantiate(_mediatorsPrefab, _gameContainer);

        _attackerSkillMediator = mediators.gameObject.AddComponent<AttackerSkillMediator>();
        _runeBoardSkillMediator = mediators.gameObject.AddComponent<RuneBoardSkillMediator>();
        _shapeSpawnerSkillMediator = mediators.gameObject.AddComponent<ShapeSpawnerSkillMediator>();
        _playFieldSkillMediator = mediators.gameObject.AddComponent<PlayFieldSkillMediator>();
    }

    public Game Create(ISaver saver, ILeaderBoard leaderBoard)
    {
        FillWindowList();

        _runeBoard = _runeBoardFactory.Create(_menu, _runeBoardSkillMediator);
        _userSkillPerformer = _skillFactory.Create(_playFieldSkillMediator);

        _attackerSkillMediator.Initialize(_runeBoardFactory.Attacker, _userSkillPerformer);
        _runeBoardSkillMediator.Initialize(_userSkillPerformer);
        _shapeSpawnerSkillMediator.Initialize(_runeBoardFactory.ShapePresenterSpawner, _userSkillPerformer);
        _playFieldSkillMediator.Initialize(_runeBoardFactory.PlayField);

        _taskFactory.Initialize(_runeBoardFactory.ShapePresenterSpawner);
        EntityDataForGame entityDataForGame = new();
        entityDataForGame.TakeModules(_runeBoard, _userSkillPerformer);
        entityDataForGame.Take(saver, _finalGameHandler, _skillFactory.UserSkillScreen, _taskFactory);

        Game game = new(entityDataForGame);

        ModuleLanguageHandler = Instantiate(_moduleLanguageHandlerPrefab, _gameContainer);
        ModuleLanguageHandler.Initialize(_runeBoardFactory.EnemyPresenter, _skillFactory.GetSkillCardPresenters());
        _finalGameHandler.Initialize(_skillFactory.SkillCardDiscoverer);
        _menu.Initialize(saver, leaderBoard, game, _skillFactory.UserSkillScreen, _runeBoardFactory.IncreasedDamageScreen);

        return game;
    }

    public List<string> GetActivatedSkills()
    {
        return _skillFactory.FillActivatedSkills();
    }

    public void Set(bool isDesktop)
    {
        _runeBoardFactory.Set(isDesktop);
        _taskFactory.Set(isDesktop);
    }

    private void FillWindowList()
    {
        _windowsWithPause.AddRange(_menu.GetWindows());
        _windowsWithPause.Add(_skillFactory.UserSkillScreen);
        _windowsWithPause.Add(_runeBoardFactory.IncreasedDamageScreen);
    }
}