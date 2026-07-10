using System;

internal class ManaGenerator
{
    private const int MultiplierForMana = 18;
    private const int MultiplierForCombo = 20;
    private const int ManaCostMultiplier = 2;

    private readonly int _manaPerCube;
    private readonly int _minSkillCost;
    private int _manaCount;
    private int _manaCountAtCurrentLevel;
    private int _comboReward;
    private int _currentLevel;
    private int _increasePerAdvertising;

    internal ManaGenerator(int manaPerCube, int minSkillCost)
    {
        if (manaPerCube <= 0)
            throw new ArgumentOutOfRangeException(nameof(manaPerCube));

        if (minSkillCost < 0)
            throw new ArgumentOutOfRangeException(nameof(minSkillCost));

        _manaPerCube = manaPerCube;
        _minSkillCost = minSkillCost;
    }

    internal event Action<int> ManaCountChanged;
    internal event Action ManaDepleted;

    internal int ManaCount => _manaCount;
    internal int ManaCostPerLevel => _currentLevel * ManaCostMultiplier;
    internal int ManaCountIncrease => Constants.ManaCountIncrease + _currentLevel;

    internal void SetComboManaReward(int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        _comboReward = count;
    }

    internal void SetStartData(int startLevel, int manaCount)
    {
        if (startLevel <= 0)
            throw new ArgumentOutOfRangeException(nameof(startLevel));

        _manaCount = manaCount;
        _manaCountAtCurrentLevel = manaCount;
        _currentLevel = startLevel;
        ManaCountChanged?.Invoke(_manaCount);
    }

    internal void Restart()
    {
        _manaCount = _manaCountAtCurrentLevel;
        ManaCountChanged?.Invoke(_manaCount);
    }

    internal bool CanSpendMana(int cost)
    {
        bool canSpend = _manaCount >= cost;

        if (!canSpend)
            ManaDepleted?.Invoke();

        return canSpend;
    }

    internal void SpendMana(int cost)
    {
        if (cost < 0)
            throw new ArgumentOutOfRangeException(nameof(cost));

        _manaCount -= cost;
        ManaCountChanged?.Invoke(_manaCount);
    }

    internal void Reward(ManaReward reward)
    {
        if (reward is ComboManaReward)
        {
            _manaCount += _comboReward + reward.Value * MultiplierForCombo;
        }
        else if (reward is ADVManaReward)
        {
            _manaCount += _increasePerAdvertising;
        }
        else if (reward is CubeManaReward)
        {
            _manaCount += reward.Value * _manaPerCube;
        }
        else if (reward is LevelManaReward)
        {
            _currentLevel = reward.Value;
            _manaCount += ManaCountIncrease;
            _manaCountAtCurrentLevel = _manaCount;
        }

        ManaCountChanged?.Invoke(_manaCount);
    }

    internal bool HaveManaForSkill()
    {
        return _manaCount >= _minSkillCost + ManaCostPerLevel;
    }

    internal int CalculateIncrease()
    {
        _increasePerAdvertising = Constants.AdvertisingReward + _currentLevel * MultiplierForMana;
        return _increasePerAdvertising;
    }
}

