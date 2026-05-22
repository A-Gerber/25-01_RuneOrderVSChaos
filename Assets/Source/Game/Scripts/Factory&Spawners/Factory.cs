using System.Collections.Generic;
using UnityEngine;

internal class Factory : MonoBehaviour
{
    [SerializeField] private AreaViewFactory _areaFactory;
    [SerializeField] private ShapePresenterSpawner _shapePresenterSpawner;
    [SerializeField] private EnemiesFactory _enemiesFactory;
    [SerializeField] private ProjectileSpawner _projectileSpawner;
    [SerializeField] private AttackerFactory _attackerFactory;
    [SerializeField] private UserSkillFactory _skillFactory;
    [SerializeField] private ManaGeneratorFactory _manaGeneratorFactory;
    [SerializeField] private StalactiteViewSpawner _stalactiteViewSpawner;
    [SerializeField] private GroundImpactEffectSpawner _groundImpactEffectSpawner;
    [SerializeField] private FreezingEffectSpawner _freezingEffectSpawner;
    [SerializeField] private EffectConfettiSpawner _effectConfettiSpawner;
    [SerializeField] private PlayerInputController _controller;
    [SerializeField] private UserSkillHandlerView _userSkillHandlerView;
    [SerializeField] private Transform _yandexEntitiesContainer;
    [SerializeField] private RectTransform _leaderBoardContainer;
    [SerializeField] private LeaderBoard _leaderBoardPrefab;
    [SerializeField] private PauseEventHandler _pauseEventHandlerPrefab;
    [SerializeField] private AdvertisementViewer _advertisementViewerPrefab;

    private readonly EntityDataForGame _entityDataForGame = new();
    private UserSkillHandler _userSkillHandler;
    private GameModel _gameModel;
    private LeaderBoard _leaderBoard;

    internal GameModel CreateGameModel()
    {
        AreaModel areaModel = _areaFactory.Create();
        AttackerModel attacker = _attackerFactory.Create();
        UserSkillPerformer userSkillPerformer = _skillFactory.CreateUserSkillPerformer();
        ManaGenerator manaGenerator = _manaGeneratorFactory.Create(_skillFactory.MinManaCost);
        _userSkillHandler = _skillFactory.CreateUserSkillHandler(attacker, manaGenerator);
        EnemySkillPerfomer enemySkillPerfomer = _enemiesFactory.Create(areaModel, _stalactiteViewSpawner, _groundImpactEffectSpawner, _freezingEffectSpawner);

        userSkillPerformer.Initialize(areaModel, _shapePresenterSpawner.GetCubesSpawner());
        _userSkillHandlerView.Initialize(_userSkillHandler);
        _projectileSpawner.Initialize(_enemiesFactory.GetEnemyPosition());

        _entityDataForGame.Take(areaModel, attacker, manaGenerator);
        _entityDataForGame.TakeCreators(_enemiesFactory, _shapePresenterSpawner, _projectileSpawner);
        _entityDataForGame.TakeEntityData(userSkillPerformer, enemySkillPerfomer);
        _entityDataForGame.TakeController(_controller);

        _gameModel = new(_entityDataForGame);

        _shapePresenterSpawner.Initialize(_gameModel, userSkillPerformer, areaModel);
        _enemiesFactory.SetGameForEnemyPresenter(_gameModel);

        return _gameModel;
    }

    internal void CreateYandexEntities(MenuView menu)
    {
        _leaderBoard = Instantiate(_leaderBoardPrefab, _leaderBoardContainer);

        Instantiate(_pauseEventHandlerPrefab, _yandexEntitiesContainer).Initialize(_userSkillHandlerView, menu);
        Instantiate(_advertisementViewerPrefab, _yandexEntitiesContainer).Initialize(menu, _gameModel);
    }

    internal void InitializeUserSkillHandler(ISettingableSkillButton gameView)
    {
        _userSkillHandler.Initialize(gameView);
    }

    internal void InitializeEffectConfettiSpawner(IWinable game)
    {
        _effectConfettiSpawner.Initialize(game);
    }

    internal List<string> GetNameOfActivatedSkills()
    {
        return _skillFactory.GetNameOfActivatedSkills();
    }

    internal EntityDataForMenu GetEntityDataForMenu(Saver saver)
    {
        EntityDataForMenu entityDataForMenu = new(_gameModel, _userSkillHandler, _skillFactory.SkillCardDiscoverer);
        entityDataForMenu.TakeYandexEntities(saver, _leaderBoard);

        return entityDataForMenu;
    }

    internal List<SkillCardView> GetSkillCardViews()
    {
        return _skillFactory.GetSkillCardViews();
    }
}