using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ConfigurationGenerator
{
    private const float IndexPerSeconds = 30f;
    private const float LevelCoefficient = 0.5f;
    private const int SecondIndex = 1;

    private readonly List<ICubeConfigurator> _configurators = new();
    private int _startLevel;
    private float _startTime;

    internal ConfigurationGenerator(int startLevel)
    {
        if (startLevel < 1)
            throw new ArgumentOutOfRangeException(nameof(startLevel));

        _startLevel = startLevel;
        _configurators.Add(new StartCubeConfigurator());
        _configurators.Add(new SimpleCubeConfigurator());
        _configurators.Add(new SimpleCubeConfigurator());
        _configurators.Add(new MiddleCubeConfigurator());
        _configurators.Add(new HardCubeConfigurator());
        _configurators.Add(new HardCubeConfigurator());
    }

    internal ICubeConfigurator GenerateConfiguration(int level)
    {
        if (level == _startLevel)
            return _configurators[0];

        int index = (int)(SecondIndex + (level * LevelCoefficient + Time.time - _startTime) / IndexPerSeconds);
        index = Math.Clamp(index, SecondIndex, _configurators.Count - 1);

        return _configurators[index];
    }

    internal void StartLevel()
    {
        _startTime = Time.time;
    }
}
