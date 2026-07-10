using System;

internal class ScoreCounter
{
    private readonly int _numberSimpleCombo;

    private int _comboInterval = 0;

    public ScoreCounter(int numberSimpleCombo)
    {
        if (numberSimpleCombo <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberSimpleCombo));

        _numberSimpleCombo = numberSimpleCombo;
    }

    internal event Action<int> ShowedScore;
    internal event Action<int> SkillPointsAwarded;
    internal event Action UpdatedTimeFrame;

    internal int NumberSimpleCombo => _numberSimpleCombo;
    internal int TotalComboNumber { get; private set; } = 0;
    internal int ComboSkillPointsInterval { get; private set; } = 5;
    internal float TimeFrameOfCombo { get; private set; } = 10f;
    internal int MaxTotalCombo { get; private set; } = 0;

    internal void SetParameters(int comboSkillPointsInterval, float timeFrameOfCombo)
    {
        if (comboSkillPointsInterval <= 0)
            throw new ArgumentOutOfRangeException(nameof(comboSkillPointsInterval));

        if (timeFrameOfCombo <= 0)
            throw new ArgumentOutOfRangeException(nameof(timeFrameOfCombo));

        ComboSkillPointsInterval = comboSkillPointsInterval;
        TimeFrameOfCombo = timeFrameOfCombo;

        UpdatedTimeFrame?.Invoke();
    }

    internal void CalculateCombo(int numberOfCombos)
    {
        TotalComboNumber += numberOfCombos;
        ShowedScore?.Invoke(numberOfCombos);


        MaxTotalCombo = Math.Max(TotalComboNumber, MaxTotalCombo);
        int comboInterval = TotalComboNumber / ComboSkillPointsInterval;

        if (TotalComboNumber % ComboSkillPointsInterval != 0 && comboInterval > _comboInterval)
        {
            SkillPointsAwarded?.Invoke(comboInterval);

        }
        else if (comboInterval > _comboInterval)
        {
            SkillPointsAwarded?.Invoke(comboInterval);
            _comboInterval = comboInterval;
        }

    }

    internal void ResetCounter()
    {
        TotalComboNumber = 0;
        _comboInterval = 0;
    }

    internal void ResetMaxScore()
    {
        MaxTotalCombo = 0;
        TotalComboNumber = 0;
        _comboInterval = 0;
    }
}
