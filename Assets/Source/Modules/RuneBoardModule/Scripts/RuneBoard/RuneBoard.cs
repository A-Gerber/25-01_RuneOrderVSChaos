using System;
using System.Collections.Generic;

public class RuneBoard
{
    private PlayField _playField;
    private EnemySkillPerfomer _enemySkillPerfomer;
    private Attacker _attacker;
    private IRuneBoardSkillMediator _mediator;
    private bool _isEnabled = true;
    private bool _isDeadEnemy;
    private int _gameScore = 0;
    private int _level;
    private int _gameScoreIncrease;

    public event Action StartedGame;
    public event Action DisablingHint;
    public event Action FinishedGame;
    public event Action<RuneBoardSavedData, int> GameWined;

    public int CurrentLevel => _level;
    public int GameScore => _gameScore;

    public void Start(RuneBoardSavedData data)
    {
        _level = data.Level;
        _gameScore = data.GameScore;
        _playField.Reset(_level);
        _attacker.Start(_level);
        _isDeadEnemy = false;
        StartedGame?.Invoke();
    }

    public void Restart()
    {
        _attacker.Restart();
        _playField.Reset(_level);
        _isDeadEnemy = false;
        StartedGame?.Invoke();
    }

    public void GoToNextLevel()
    {
        StartedGame?.Invoke();
        _attacker.Start(_level);
        _playField.Reset(_level);
        _isDeadEnemy = false;
    }

    internal void Set(bool isEnabled)
    {
        _isEnabled = isEnabled;
        _playField.ShapePlatform.Set(isEnabled);
    }

    internal void Initialize(EntityDataForRuneBoard entityDataForRuneBoard)
    {
        if (_enemySkillPerfomer != null)
            _enemySkillPerfomer.PlacedStalactite -= CheckOverGame;

        if (_attacker != null)
            _attacker.CubesReleased -= CheckOverGame;

        if (_mediator != null)
        {
            _mediator.RuneBoardChecking -= CheckOverGame;
            _mediator.RuneBoardAttacking -= TryAttack;
            _mediator.RuneBoardReleasingTargets -= () =>
            {
                if (_playField.TryReleaseTargets(out List<LocalPosition> targetPositions))
                    _attacker.Attack(targetPositions);
            };
        }

        if (entityDataForRuneBoard == null)
            throw new ArgumentNullException("entityDataForRuneBoard is null", nameof(entityDataForRuneBoard));

        _playField = entityDataForRuneBoard.PlayField ?? throw new ArgumentNullException("Area is null", nameof(entityDataForRuneBoard.PlayField));
        _attacker = entityDataForRuneBoard.Attacker ?? throw new ArgumentNullException("Attacker is null", nameof(entityDataForRuneBoard.Attacker));
        _enemySkillPerfomer = entityDataForRuneBoard.EnemySkillPerfomer ?? throw new ArgumentNullException("EnemySkillPerfomer is null", nameof(entityDataForRuneBoard.EnemySkillPerfomer));
        _mediator = entityDataForRuneBoard.Mediator ?? throw new ArgumentNullException("Mediator is null", nameof(entityDataForRuneBoard.Mediator));

        if (_enemySkillPerfomer != null)
            _enemySkillPerfomer.PlacedStalactite += CheckOverGame;

        if (_attacker != null)
            _attacker.CubesReleased += CheckOverGame;

        if (_mediator != null)
        {
            _mediator.RuneBoardChecking += CheckOverGame;
            _mediator.RuneBoardAttacking += TryAttack;
            _mediator.RuneBoardReleasingTargets += () =>
            {
                if (_playField.TryReleaseTargets(out List<LocalPosition> targetPositions))
                    _attacker.Attack(targetPositions);
            };
        }
    }

    internal void ProcessStep()
    {
        _playField.ShapePlatform.CreateShapes(_level);
        TryAttack();
        CheckOverGame();
    }

    private void TryAttack()
    {
        if (_playField.TryReleaseCellsByLines(out List<LocalPosition> targetPositions))
            _attacker.Attack(targetPositions);
    }

    private void CheckOverGame()
    {
        if (_isEnabled == false || _isDeadEnemy)
            return;

        if (_attacker.IsAliveEnemy == false)
        {
            Win();
            return;
        }

        if (_playField.IsLostGame())
        {
            FinishedGame?.Invoke();
            return;
        }

        DisablingHint?.Invoke();
    }

    private void Win()
    {
        _isDeadEnemy = true;
        _gameScoreIncrease = _attacker.ScoreIncrease + _mediator.ManaCount;
        _level++;
        _gameScore += _gameScoreIncrease;

        GameWined?.Invoke(new RuneBoardSavedData(_level, _gameScore), _gameScoreIncrease);
    }
}
