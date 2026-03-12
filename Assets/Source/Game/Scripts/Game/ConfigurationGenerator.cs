using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

internal class ConfigurationGenerator
{
    private const float IndexPerSeconds = 30f;
    private const float IndexPerSecondsForLastLevel = 10f;
    private const int Coefficient = 4;
    private const int LevelDivider = 10;
    private const int SecondIndex = 1;

    private readonly List<ICubeConfigurator> _configurators = new();
    private int _startLevel;
    private float _startTime;

    internal ConfigurationGenerator(int startLevel)
    {
        if (startLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(startLevel));

        _startLevel = startLevel;
        //_configurators.Add(new StartCubeConfigurator());
        _configurators.Add(new VerySimpleCubeConfigurator());
        _configurators.Add(new SimpleCubeConfigurator());
        _configurators.Add(new UsualCubeConfigurator());
        _configurators.Add(new MiddleCubeConfigurator());
        _configurators.Add(new HardCubeConfigurator());
        _configurators.Add(new VeryHardCubeConfigurator());
        _configurators.Add(new UltrayHardCubeConfigurator());
    }

    internal ICubeConfigurator GetCubeConfigurator(int level)
    {
        //if (level <= 0) // _startLevel
        //return _configurators[0];

        //Debug.Log("Убрать комменты");

        if (level <= Constants.LastLevel)
        {
            int dividedLevel = level/ LevelDivider;
            int index = (int)((Time.time - _startTime) / (IndexPerSeconds - dividedLevel * Coefficient));
            int maxIndex = Math.Clamp(_configurators.Count - (_configurators.Count - 1 - dividedLevel), 0, _configurators.Count - 1);
            index = Math.Clamp(index, 0, maxIndex);

            return _configurators[index];
        }
        else
        {
            int index = (int)((Time.time - _startTime) / IndexPerSecondsForLastLevel);
            index = Math.Clamp(index, SecondIndex, _configurators.Count - 1);

            return _configurators[index];
        }
    }

    internal void StartLevel()
    {
        _startTime = Time.time;
    }
}
