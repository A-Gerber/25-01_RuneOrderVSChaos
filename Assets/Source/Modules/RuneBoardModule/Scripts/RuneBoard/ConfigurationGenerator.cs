using System;
using System.Collections.Generic;
using UnityEngine;

internal class ConfigurationGenerator : MonoBehaviour
{
    private const float IndexPerSeconds = 30f;
    private const float IndexPerSecondsForLastLevel = 10f;
    private const int Coefficient = 4;
    private const int LevelDivider = 10;
    private const int SecondIndex = 1;

    private readonly List<ICubeConfigurator> _configurators = new();

    private float _elapsedTime = 0f;
    private bool _isRunning = false;

    private void Awake()
    {
        _configurators.Add(new VerySimpleCubeConfigurator());
        _configurators.Add(new SimpleCubeConfigurator());
        _configurators.Add(new UsualCubeConfigurator());
        _configurators.Add(new MiddleCubeConfigurator());
        _configurators.Add(new HardCubeConfigurator());
        _configurators.Add(new VeryHardCubeConfigurator());
        _configurators.Add(new UltrayHardCubeConfigurator());
    }

    private void Update()
    {
        if (_isRunning)
            _elapsedTime += Time.deltaTime;
    }

    internal ICubeConfigurator GetCubeConfigurator(int level)
    {
        if (level <= Constants.LastLevel)
        {
            int dividedLevel = level / LevelDivider;
            int index = (int)(_elapsedTime / (IndexPerSeconds - dividedLevel * Coefficient));
            int maxIndex = Math.Clamp(_configurators.Count - (_configurators.Count - 1 - dividedLevel), 0, _configurators.Count - 1);
            index = Math.Clamp(index, 0, maxIndex);

            return _configurators[index];
        }
        else
        {
            int index = (int)(_elapsedTime / IndexPerSecondsForLastLevel);
            index = Math.Clamp(index, SecondIndex, _configurators.Count - 1);

            return _configurators[index];
        }
    }

    internal void ResetTimeCounter()
    {
        _elapsedTime = 0;
    }

    internal void StartCountdown()
    {
        _isRunning = true;
    }
}
