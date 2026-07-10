using System;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour, IOpenable
{
    private readonly List<Window> _windowsWhithPause = new();

    [SerializeField] private MenuScreen _menuScreen;
    [SerializeField] private SkillTooltipScreen _skillTooltipScreen;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private LeaderboardScreen _leaderboardScreen;

    private IncreasedDamageScreen _increasedDamageScreen;
    private ILeaderBoard _leaderBoard;
    private IGettableLevel _saver;
    private IReportableOpenEvent _userSkillScreen;
    private Game _game;

    private void OnEnable()
    {
        _settingsScreen.ExitButtonClicked += () => _settingsScreen.Close();
        _skillTooltipScreen.ExitButtonClicked += () => _skillTooltipScreen.Close();

        _leaderboardScreen.ExitButtonClicked += () =>
        {
            _leaderboardScreen.Close();
            _leaderBoard.SetActive(false);
        };

        _menuScreen.BackButtonClicked += () => _menuScreen.Close();
        _menuScreen.SettingsButtonClicked += () => _settingsScreen.Open();

        _menuScreen.NewGameButtonClicked += () =>
        {
            _game.StartNewGame();
            _menuScreen.Close();
        };

        _menuScreen.ContinueButtonClicked += () =>
        {
            _game.Start();
            _menuScreen.Close();
        };

        _menuScreen.LeaderboardButtonClicked += () =>
        {
            _leaderBoard.SetActive(true);
            _leaderboardScreen.Open();
        };
    }

    private void OnDisable()
    {
        _settingsScreen.ExitButtonClicked -= () => _settingsScreen.Close();
        _skillTooltipScreen.ExitButtonClicked -= () => _skillTooltipScreen.Close();

        _leaderboardScreen.ExitButtonClicked += () =>
        {
            _leaderboardScreen.Close();
            _leaderBoard.SetActive(false);
        };

        _menuScreen.BackButtonClicked -= () => _menuScreen.Close();
        _menuScreen.SettingsButtonClicked -= () => _settingsScreen.Open();

        _menuScreen.NewGameButtonClicked -= () =>
        {
            _game.StartNewGame();
            _menuScreen.Close();
        };

        _menuScreen.ContinueButtonClicked -= () =>
        {
            _game.Start();
            _menuScreen.Close();
        };

        _menuScreen.LeaderboardButtonClicked -= () =>
        {
            _leaderBoard.SetActive(true);
            _leaderboardScreen.Open();
        };
    }

    public void Initialize(IGettableLevel saver, ILeaderBoard leaderBoard, Game game, IReportableOpenEvent userSkillScreen, IncreasedDamageScreen increasedDamageScreen)
    {
        if(_game != null)
            _game.SetedNewRecord -= (gameScore) => _leaderBoard.SaveResult(gameScore);

        if (_userSkillScreen != null)
            _userSkillScreen.Opened -= OnOpenSkillScreen;

        if (_increasedDamageScreen != null)
            _increasedDamageScreen.Opened -= () => {if(_skillTooltipScreen.IsOpen) _skillTooltipScreen.Close(); };

        _saver = saver ?? throw new ArgumentNullException("saver is null", nameof(saver));
        _leaderBoard = leaderBoard ?? throw new ArgumentNullException("leaderBoard is null", nameof(leaderBoard));
        _game = game ?? throw new ArgumentNullException("game is null", nameof(game));
        _userSkillScreen = userSkillScreen ?? throw new ArgumentNullException("userSkillScreen is null", nameof(userSkillScreen));
        _increasedDamageScreen = increasedDamageScreen != null ? increasedDamageScreen : throw new ArgumentNullException("increasedDamageScreen is null", nameof(increasedDamageScreen));

        if (_game != null)
            _game.SetedNewRecord += (gameScore) => _leaderBoard.SaveResult(gameScore);

        if (_userSkillScreen != null)
            _userSkillScreen.Opened += OnOpenSkillScreen;

        if (_increasedDamageScreen != null)
            _increasedDamageScreen.Opened += () => { if (_skillTooltipScreen.IsOpen) _skillTooltipScreen.Close(); }; ;
    }

    public void Open()
    {
        if (_skillTooltipScreen.IsOpen)
            _skillTooltipScreen.Close();

        if (_increasedDamageScreen.IsOpen)
            _increasedDamageScreen.Close();

        _menuScreen.SetActiveCloseButton(_game.IsPlaying);
        _menuScreen.SetInteractableContinueButton(_saver.CurrentLevel > Constants.StartLevel);
        _menuScreen.Open();
    }

    public void OpenTooltip()
    {
        if (_increasedDamageScreen.IsOpen)
            _increasedDamageScreen.Close();

        _skillTooltipScreen.Open();
    }

    internal IReadOnlyList<IWindowController> GetWindows()
    {
        _windowsWhithPause.Add(_menuScreen);
        _windowsWhithPause.Add(_skillTooltipScreen);

        return _windowsWhithPause;
    }

    private void OnOpenSkillScreen()
    {
        if (_skillTooltipScreen.IsOpen) 
            _skillTooltipScreen.Close();

        if (_increasedDamageScreen.IsOpen)
            _increasedDamageScreen.Close();
    }
}
