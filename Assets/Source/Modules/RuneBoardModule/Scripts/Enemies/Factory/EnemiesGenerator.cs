using System;
using System.Collections.Generic;

internal class EnemiesGenerator
{
    private const int FirstMinOfFirstList = 6;
    private const int FirstMaxOfFirstList = 9;
    private const int ValueOfFirstList = 15;
    private const int SecondMinOfFirstList = 17;
    private const int SecondMaxOfFirstList = 19;
    private const int ThirdMinOfFirstList = 23;
    private const int ThirdMaxOfFirstList = 24;

    private const int FirstMinOfSecondList = 26;
    private const int FirstMaxOfSecondList = 29;

    private const int FirstMinOfThirdList = 33;
    private const int FirstMaxOfThirdList = 36;
    private const int SecondMinOfThirdList = 38;
    private const int SecondMaxOfThirdList = 39;

    private const int FirstMinOfFourthList = 42;
    private const int FirstMaxOfFourthList = 45;
    private const int SecondMinOfFourthList = 47;
    private const int SecondMaxOfFourthList = 49;

    private readonly IReadOnlyList<IEnemy> _enemies;
    private readonly List<IEnemy> _firstEnemyList = new();
    private readonly List<IEnemy> _secondEnemyList = new();
    private readonly List<IEnemy> _thirdEnemyList = new();
    private readonly List<IEnemy> _fourthEnemyList = new();
    private readonly List<IEnemy> _fifthEnemyList = new();

    private readonly List<int> _goblinLevels;
    private readonly List<int> _orcLevels;
    private readonly List<int> _orcChieftain;
    private readonly List<int> _yetiLevels;
    private readonly List<int> _fenrirLevels;
    private readonly List<int> _snowQueenLevels;
    private readonly List<int> _gargoyleLevels;
    private readonly List<int> _earthDragonLevels;


    public EnemiesGenerator(List<IEnemy> enemies)
    {
        _enemies = enemies ?? throw new InvalidOperationException("enemies is null");

        _goblinLevels = new() { { 1 }, { 2 }, { 4 }, { 11 } };
        _orcLevels = new() { { 14 }, { 22 }, { 32 } };
        _orcChieftain = new() { { 10 }, { 16 }, { 25 }, { 41 } };
        _yetiLevels = new() { { 3 }, { 13 } };
        _fenrirLevels = new() { { 20 }, { 31 } };
        _snowQueenLevels = new() { { 40 }, { 46 } };
        _gargoyleLevels = new() { { 5 }, { 12 }, { 21 } };
        _earthDragonLevels = new() { { 30 }, { 37 } };

        FillRandomLists();
    }

    internal IEnemy Generate(int level)
    {
        if (_goblinLevels.Contains(level))
            return _enemies[(int)Enemies.Goblin];
        else if (_orcLevels.Contains(level))
            return _enemies[(int)Enemies.Orc];
        else if (_yetiLevels.Contains(level))
            return _enemies[(int)Enemies.Yeti];
        else if (_orcChieftain.Contains(level))
            return _enemies[(int)Enemies.OrcChieftain];
        else if (_fenrirLevels.Contains(level))
            return _enemies[(int)Enemies.Fenrir];
        else if (_gargoyleLevels.Contains(level))
            return _enemies[(int)Enemies.Gargoyle];
        else if (_snowQueenLevels.Contains(level))
            return _enemies[(int)Enemies.SnowQueen];
        else if (_earthDragonLevels.Contains(level))
            return _enemies[(int)Enemies.EarthDragon];
        else if (level == Constants.LastLevel)
            return _enemies[8];
        else if (IsBelongToFirstList(level))
            return _firstEnemyList[UnityEngine.Random.Range(0, _firstEnemyList.Count)];
        else if (UserUtilities.IsInRangeInt(level, FirstMinOfSecondList, FirstMaxOfSecondList))
            return _secondEnemyList[UnityEngine.Random.Range(0, _secondEnemyList.Count)];
        else if (UserUtilities.IsInRangeInt(level, FirstMinOfThirdList, FirstMaxOfThirdList) || UserUtilities.IsInRangeInt(level, SecondMinOfThirdList, SecondMaxOfThirdList))
            return _thirdEnemyList[UnityEngine.Random.Range(0, _thirdEnemyList.Count)];
        else if (UserUtilities.IsInRangeInt(level, FirstMinOfFourthList, FirstMaxOfFourthList) || UserUtilities.IsInRangeInt(level, SecondMinOfFourthList, SecondMaxOfFourthList))
            return _fourthEnemyList[UnityEngine.Random.Range(0, _fourthEnemyList.Count)];
        else
            return _fifthEnemyList[UnityEngine.Random.Range(0, _fifthEnemyList.Count)];
    }

    private void FillRandomLists()
    {
        _firstEnemyList.Add(_enemies[(int)Enemies.Orc]);
        _firstEnemyList.Add(_enemies[(int)Enemies.Yeti]);
        _firstEnemyList.Add(_enemies[(int)Enemies.Gargoyle]);

        _secondEnemyList.Add(_enemies[(int)Enemies.Orc]);
        _secondEnemyList.Add(_enemies[(int)Enemies.Fenrir]);
        _secondEnemyList.Add(_enemies[(int)Enemies.Gargoyle]);

        _thirdEnemyList.Add(_enemies[(int)Enemies.OrcChieftain]);
        _thirdEnemyList.Add(_enemies[(int)Enemies.Fenrir]);
        _thirdEnemyList.Add(_enemies[(int)Enemies.Gargoyle]);

        _fourthEnemyList.Add(_enemies[(int)Enemies.OrcChieftain]);
        _fourthEnemyList.Add(_enemies[(int)Enemies.Fenrir]);
        _fourthEnemyList.Add(_enemies[(int)Enemies.EarthDragon]);

        _fifthEnemyList.Add(_enemies[(int)Enemies.OrcChieftain]);
        _fifthEnemyList.Add(_enemies[(int)Enemies.EarthDragon]);
        _fifthEnemyList.Add(_enemies[(int)Enemies.SnowQueen]);
        _fifthEnemyList.Add(_enemies[(int)Enemies.WitchOfChaos]);
    }

    private bool IsBelongToFirstList(int level)
    {
        bool isBelongToFirstInterval = UserUtilities.IsInRangeInt(level, FirstMinOfFirstList, FirstMaxOfFirstList);
        bool isBelongToSecondInterval = UserUtilities.IsInRangeInt(level, SecondMinOfFirstList, SecondMaxOfFirstList);
        bool isBelongToThirdInterval = UserUtilities.IsInRangeInt(level, ThirdMinOfFirstList, ThirdMaxOfFirstList);

        return (isBelongToFirstInterval || level == ValueOfFirstList || isBelongToSecondInterval || isBelongToThirdInterval);
    }
}