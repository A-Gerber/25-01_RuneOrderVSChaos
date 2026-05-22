using System;

internal class ManaGenerator : ISettableComboManaReward
{
    private const int MultiplierForMana = 5;
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

    public void SetComboManaReward(int count)
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

    internal void RewardForLevel(int currentLevel)
    {
        if (currentLevel <= 0)
            throw new ArgumentOutOfRangeException(nameof(currentLevel));

        _currentLevel = currentLevel;
        _manaCount += Constants.ManaCountIncrease + currentLevel;
        _manaCountAtCurrentLevel = _manaCount;
        ManaCountChanged?.Invoke(_manaCount);
    }

    internal void RewardForCubes(int cubesCount)
    {
        if (cubesCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(cubesCount));

        _manaCount += cubesCount * _manaPerCube;
        ManaCountChanged?.Invoke(_manaCount);
    }

    internal void RewardForCombo(int numberOfRewards)
    {
        if (numberOfRewards < 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfRewards));

        _manaCount += _comboReward + numberOfRewards * MultiplierForMana;
        ManaCountChanged?.Invoke(_manaCount);
    }

    internal void RewardForAdvertising()
    {
        _manaCount += _increasePerAdvertising;
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

public interface ISettableComboManaReward
{
    void SetComboManaReward(int comboManaReward);
}