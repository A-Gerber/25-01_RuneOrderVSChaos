using TMPro;
using UnityEngine;
using YG;

public class LevelInstaller : MonoBehaviour
{
    [SerializeField] private ConstantsInstaller _constantsInstaller;
    [SerializeField] private GameFactory _gameFactory;
    [SerializeField] private PlayerInputController _controller;
    [SerializeField] private TMP_Dropdown _languagDropdown;

    [SerializeField] private Transform _yandexEntitiesContainer;
    [SerializeField] private LanguageHandler _languageHandlerPrefab;
    [SerializeField] private Saver _saverPrefab;
    [SerializeField] private PauseEventHandler _pauseEventHandlerPrefab;
    [SerializeField] private AdvertisementViewer _advertisementViewerPrefab;
    [SerializeField] private LeaderBoard _leaderBoardPrefab;
    [SerializeField] private RectTransform _leaderBoardContainer;

    private void Awake()
    {
        _constantsInstaller.SetConstants();
        _constantsInstaller.SetLanguage(YG2.lang);
        _gameFactory.Set(YG2.envir.isDesktop);

        Saver saver = Instantiate(_saverPrefab, _yandexEntitiesContainer);
        LanguageHandler languageHandler = Instantiate(_languageHandlerPrefab, _yandexEntitiesContainer);

        _gameFactory.CreateMediators();
        Game game = _gameFactory.Create(saver, Instantiate(_leaderBoardPrefab, _leaderBoardContainer));

        Instantiate(_pauseEventHandlerPrefab, _yandexEntitiesContainer).Initialize(_gameFactory.WindowsWithPause);
        Instantiate(_advertisementViewerPrefab, _yandexEntitiesContainer).Initialize(game, _gameFactory.FinalGameHandler, _gameFactory.AttackerPresenter);

        RuneBoardSavedData runeBoardStartData = new(Constants.StartLevel, _constantsInstaller.StartGameScore);
        UserSkillSavedData userSkillStartData = new(_constantsInstaller.StartManaCount, _gameFactory.GetActivatedSkills());
        saver.SetStartData(runeBoardStartData, userSkillStartData);

        _controller.Initialize(_gameFactory.UserSkillPerformerPresenter);
        languageHandler.Initialize(_gameFactory.ModuleLanguageHandler, _languagDropdown);
    }

    private void Start()
    {
        _gameFactory.Menu.Open();
    }
}
