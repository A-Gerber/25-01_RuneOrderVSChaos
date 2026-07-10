using System;

public class Game
{
    private readonly RuneBoard _runeBoard;
    private readonly UserSkillPerformer _userSkillPerformer;
    private readonly ISaver _saver;
    private readonly FinalGameHandler _finalGameHandler;
    private readonly TaskFactory _taskFactory;
    private TaskHandler _taskHandler;

    public Game(EntityDataForGame data)
    {
        if (data == null)
            throw new ArgumentNullException("data is null", nameof(data));

        _runeBoard = data.RuneBoard ?? throw new ArgumentNullException("RuneBoard is null", nameof(data.RuneBoard));
        _userSkillPerformer = data.UserSkillPerformer ?? throw new ArgumentNullException("UserSkillPerformer is null", nameof(data.UserSkillPerformer));
        _saver = data.Saver ?? throw new ArgumentNullException("Saver is null", nameof(data.Saver));
        _finalGameHandler = data.FinalGameHandler != null ? data.FinalGameHandler : throw new ArgumentNullException("FinalGameHandler is null", nameof(data.FinalGameHandler));
        _taskFactory = data.TaskFactory != null ? data.TaskFactory : throw new ArgumentNullException("TaskFactory is null", nameof(data.TaskFactory));

        _runeBoard.GameWined += OnWin;
        _runeBoard.FinishedGame += OnFinish;
        _runeBoard.DisablingHint += () => _userSkillPerformer.ChangeStateHint(false);

        _finalGameHandler.NextLevelButtonClicked += OnNextButtonClick;
        _finalGameHandler.RestartButtonClicked += OnRestart;

        data.UserSkillScreen.SavedSkills += _saver.SaveActivatedUserSkills;
    }

    internal event Action<int> SetedNewRecord;

    internal bool IsPlaying { get; private set; } = false;

    public void RewardForADV()
    {
        _userSkillPerformer.RewardWithMana(new ADVManaReward(0));
    }

    internal void StartNewGame()
    {
        _runeBoard.Start(_saver.GetStartRuneBoardSavedData());
        _userSkillPerformer.Start(_saver.GetStartSkillData(), _runeBoard.CurrentLevel);
        IsPlaying = true;

        if (_taskHandler != null)
            _taskHandler.CloseTutorial();

        _taskHandler = _taskFactory.Create(_userSkillPerformer);
    }

    internal void Start()
    {
        _runeBoard.Start(_saver.GetRuneBoardSavedData());
        _userSkillPerformer.Start(_saver.GetSkillSavedData(), _runeBoard.CurrentLevel);
        IsPlaying = true;
    }

    private void OnWin(RuneBoardSavedData data, int scoreIncrease)
    {
        _userSkillPerformer.RewardWithMana(new LevelManaReward(_runeBoard.CurrentLevel));
        _finalGameHandler.Win(_runeBoard.CurrentLevel, scoreIncrease, _userSkillPerformer.ManaCountIncrease);

        if (data.GameScore > _saver.MaxGameResult)
            SetedNewRecord?.Invoke(data.GameScore);

        _saver.SaveRuneBoardData(data);
        _saver.SaveManaCountOfUserSkill(_userSkillPerformer.ManaCount);
        _saver.SaveActivatedUserSkills(_userSkillPerformer.GetSkillsToSave());

        _saver.Save();
    }

    private void OnFinish()
    {
        if (_userSkillPerformer.HaveManaForSkill)
            _userSkillPerformer.ChangeStateHint(true);
        else
            _finalGameHandler.Finish(_userSkillPerformer.ManaIncreaseForADV);
    }

    private void OnRestart()
    {
        _runeBoard.Restart();
        _userSkillPerformer.Restart();
    }

    private void OnNextButtonClick()
    {
        _runeBoard.GoToNextLevel();
        _userSkillPerformer.GoToNextLevel(_saver.GetSkillSavedData(), _runeBoard.CurrentLevel);
    }
}
