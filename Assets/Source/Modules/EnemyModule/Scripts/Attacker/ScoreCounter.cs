using System;
using UnityEngine;

public class ScoreCounter
{
    private int _comboSkillPointsInterval;
    private float _timeFrameOfCombo;
    private int _maxTotalCombo = 0;
    private int _totalComboNumber = 0;
    private int _comboInterval = 0;

    private readonly int _numberSimpleCombo;

    public ScoreCounter(int numberSimpleCombo)
    {
        if (numberSimpleCombo <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberSimpleCombo));

        _numberSimpleCombo = numberSimpleCombo;
    }

    internal event Action<int> ShowedScore;
    internal event Action<int> SkillPointsAwarded;
    internal event Action UpdatedTimeFrame;

    internal int TotalComboNumber => _totalComboNumber;
    internal int ComboSkillPointsInterval => _comboSkillPointsInterval;
    internal int NumberSimpleCombo => _numberSimpleCombo;
    internal float TimeFrameOfCombo => _timeFrameOfCombo;
    internal int MaxTotalCombo => _maxTotalCombo;

    public void SetParameters( int comboSkillPointsInterval, float timeFrameOfCombo)
    {
        if (comboSkillPointsInterval <= 0)
            throw new ArgumentOutOfRangeException(nameof(comboSkillPointsInterval));

        if (timeFrameOfCombo <= 0)
            throw new ArgumentOutOfRangeException(nameof(timeFrameOfCombo));

        _comboSkillPointsInterval = comboSkillPointsInterval;
        _timeFrameOfCombo = timeFrameOfCombo;

        UpdatedTimeFrame?.Invoke();
    }

    internal void CalculateCombo(int numberOfCombos)
    {
        _totalComboNumber += numberOfCombos;
        ShowedScore?.Invoke(numberOfCombos);

        _maxTotalCombo = Math.Max(_totalComboNumber, _maxTotalCombo);
         int comboInterval = _totalComboNumber / _comboSkillPointsInterval;

        if (_totalComboNumber % _comboSkillPointsInterval == 0)
        {
            SkillPointsAwarded?.Invoke(comboInterval);
        }
        else
        {
            if (comboInterval > _comboInterval)
            {
                SkillPointsAwarded?.Invoke(comboInterval);
                _comboInterval = comboInterval;
            }
        }

    }

    internal void ResetCounter()
    {
        _totalComboNumber = 0;
        _comboInterval = 0;
    }

    internal void ResetMaxScore()
    {
        _maxTotalCombo = 0;
        _totalComboNumber = 0;
        _comboInterval = 0;
    }
}