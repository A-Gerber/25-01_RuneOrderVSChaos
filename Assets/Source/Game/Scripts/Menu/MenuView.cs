using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal class MenuView : MonoBehaviour, IOpenableMenu, IWindowController, IAdvertisementViewer
{
    private const string MenuPauseKey = "MenuPause";

    [SerializeField] private WinGameScreen _winGameScreen;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private MenuScreen _menuScreen;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private SkillTooltipScreen _skillTooltipScreen;
    [SerializeField] private LeaderboardScreen _leaderboardScreen;
    [SerializeField] private TextMeshProUGUI _winnerText;
    [SerializeField] private float _delay = 0.5f;

    private Saver _saver;
    private LeaderBoard _leaderBoard;
    private IGame _game;
    private IUserSkillHandler _userSkillHandler;
    private ISkillCardDiscoverer _skillCardDiscoverer;
    private WaitForSeconds _wait;

    private int _startLevel;

    public event Action<string> OpenedWindow;
    public event Action<string> ClosedWindow;
    public event Action ClickedNextButton;
    public event Action ClickedRewardButton;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
    }

    private void OnEnable()
    {
        SubscribeToScreens();
        Subscribe();
    }

    private void OnDisable()
    {
        UnsubscribeToScreens();
        Unsubscribe();
    }

    public void OpenMenu()
    {
        _menuScreen.SetActiveCloseButton(_game.IsPlaying);
        _menuScreen.SetInteractableContinueButton(_saver.CurrentLevel > _startLevel);
        _menuScreen.Open();

        OpenedWindow?.Invoke(MenuPauseKey);
    }

    public void OpenSkillsToolTip()
    {
        _skillTooltipScreen.Open();

        OpenedWindow?.Invoke(MenuPauseKey);
    }

    internal void Initialize(EntityDataForMenu entityDataForMenu)
    {
        if (entityDataForMenu == null)
            throw new InvalidOperationException("entityDataForGame is null");

        Unsubscribe();

        _game = entityDataForMenu.Game ?? throw new InvalidOperationException("game is null");
        _userSkillHandler = entityDataForMenu.UserSkillHandler ?? throw new InvalidOperationException("userSkillHandler is null");
        _skillCardDiscoverer = entityDataForMenu.SkillCardDiscoverer ?? throw new InvalidOperationException("skillCardDiscoverer is null");
        _saver = entityDataForMenu.Saver != null ? entityDataForMenu.Saver : throw new InvalidOperationException("saver is null");
        _leaderBoard = entityDataForMenu.LeaderBoard != null ? entityDataForMenu.LeaderBoard : throw new InvalidOperationException("leaderBoard is null");

        Subscribe();

        _startLevel = Constants.StartLevel;
    }

    private void OnOpenSettings()
    {
        _settingsScreen.Open();
    }

    private void OnCloseSettings()
    {
        _settingsScreen.Close();
    }

    private void OnCloseSkillTooltip()
    {
        _skillTooltipScreen.Close();

        ClosedWindow?.Invoke(MenuPauseKey);
    }

    private void OnOpenLeaderboard()
    {
        _leaderBoard.gameObject.SetActive(true);
        _leaderboardScreen.Open();
    }

    private void OnCloseLeaderboardScreen()
    {
        _leaderboardScreen.Close();
        _leaderBoard.gameObject.SetActive(false);
    }

    private void OnNewGameButtonClick()
    {
        _game.StartGame(_saver.GetStartGameData());

        _userSkillHandler.ChangeLevel(Constants.StartLevel);
        _userSkillHandler.StartGame(_saver.GetStartSkillData());

        _saver.SaveGameData(_saver.GetStartGameData());
        _saver.SaveSkillData(_saver.GetStartSkillData());
        _saver.Save();
        Debug.Log("MaxResult - " + _saver.MaxGameResult);

        _menuScreen.Close();
        ClosedWindow?.Invoke(MenuPauseKey);
        ClickedNextButton?.Invoke();
    }

    private void OnContinueButtonClick()
    {
        _game.StartGame(_saver.GetGameSavedData());
        _userSkillHandler.ChangeLevel(_game.CurrentLevel);
        _userSkillHandler.StartGame(_saver.GetSkillSavedData());

        _menuScreen.Close();
        ClosedWindow?.Invoke(MenuPauseKey);
    }

    private void OnBackButtonClick()
    {
        _menuScreen.Close();
        ClosedWindow?.Invoke(MenuPauseKey);
    }

    private void OnNextLevelButtonClick()
    {
        _game.GoToNextLevel();
        _userSkillHandler.ChangeLevel(_game.CurrentLevel);
        _winGameScreen.Close();

        ClosedWindow?.Invoke(MenuPauseKey);
        ClickedNextButton?.Invoke();
    }

    private void OnRestartButtonClick()
    {
        _game.Restart();

        _endGameScreen.Close();
        ClosedWindow?.Invoke(MenuPauseKey);
    }

    private void OnRewardButtonClick()
    {
        _endGameScreen.Close();

        ClosedWindow?.Invoke(MenuPauseKey);
        ClickedRewardButton?.Invoke();
    }

    private void OnGameOver(int manaIncrease)
    {
        _endGameScreen.ChangeManaIncrease(manaIncrease);
        _endGameScreen.Open();

        OpenedWindow?.Invoke(MenuPauseKey);
    }

    private void OnWinGame(GameSavedData data)
    {
        StartCoroutine(OpenWinScreenOverTime());

        if (_skillCardDiscoverer.TryGetSkillSprites(out List<Sprite> sprites, _game.CurrentLevel))
            _winGameScreen.ShowOpenSkills(sprites);
        else
            _winGameScreen.HideSkills(_skillCardDiscoverer.GetNextThreshold(_game.CurrentLevel), _game.CurrentLevel);

        if (data.GameScore > _saver.MaxGameResult)
            _leaderBoard.SaveResult(data.GameScore);

        _winGameScreen.UpdateIncreases(data.GameScore - _saver.GameScore, _game.CurrentLevel);
        _winnerText.gameObject.SetActive(true);
        _saver.SaveGameData(data);
        _saver.SaveSkillData(_userSkillHandler.GetSkillsToSave());
        _saver.Save();

        if (_game.CurrentLevel == Constants.LastLevel)
            _winGameScreen.ShowWitch();
        else
            _winGameScreen.HideWitch();
    }

    private void OnSaveSkills(SkillsSavedData data)
    {
        _saver.SaveSkillData(data);
    }

    private IEnumerator OpenWinScreenOverTime()
    {
        yield return _wait;
        yield return _wait;
        _winnerText.gameObject.SetActive(false);
        _winGameScreen.Open();
        OpenedWindow?.Invoke(MenuPauseKey);
    }

    private void Subscribe()
    {
        if (_game != null)
        {
            _game.GameOvered += OnGameOver;
            _game.GameWined += OnWinGame;
        }

        if (_userSkillHandler != null)
            _userSkillHandler.SavedSkills += OnSaveSkills;
    }

    private void Unsubscribe()
    {
        if (_game != null)
        {
            _game.GameOvered -= OnGameOver;
            _game.GameWined -= OnWinGame;
        }

        if (_userSkillHandler != null)
            _userSkillHandler.SavedSkills -= OnSaveSkills;
    }

    private void SubscribeToScreens()
    {
        _winGameScreen.NextLevelButtonClicked += OnNextLevelButtonClick;
        _endGameScreen.RestartButtonClicked += OnRestartButtonClick;
        _endGameScreen.RewardButtonClicked += OnRewardButtonClick;
        _settingsScreen.ExitButtonClicked += OnCloseSettings;
        _skillTooltipScreen.ExitButtonClicked += OnCloseSkillTooltip;
        _leaderboardScreen.ExitButtonClicked += OnCloseLeaderboardScreen;

        _menuScreen.NewGameButtonClicked += OnNewGameButtonClick;
        _menuScreen.ContinueButtonClicked += OnContinueButtonClick;
        _menuScreen.BackButtonClicked += OnBackButtonClick;
        _menuScreen.SettingsButtonClicked += OnOpenSettings;
        _menuScreen.LeaderboardButtonClicked += OnOpenLeaderboard;
    }

    private void UnsubscribeToScreens()
    {
        _winGameScreen.NextLevelButtonClicked -= OnNextLevelButtonClick;
        _endGameScreen.RestartButtonClicked -= OnRestartButtonClick;
        _endGameScreen.RewardButtonClicked -= OnRewardButtonClick;
        _settingsScreen.ExitButtonClicked -= OnCloseSettings;
        _skillTooltipScreen.ExitButtonClicked -= OnCloseSkillTooltip;
        _leaderboardScreen.ExitButtonClicked += OnCloseLeaderboardScreen;

        _menuScreen.NewGameButtonClicked -= OnNewGameButtonClick;
        _menuScreen.ContinueButtonClicked -= OnContinueButtonClick;
        _menuScreen.BackButtonClicked -= OnBackButtonClick;
        _menuScreen.SettingsButtonClicked -= OnOpenSettings;
        _menuScreen.LeaderboardButtonClicked -= OnOpenLeaderboard;
    }
}
