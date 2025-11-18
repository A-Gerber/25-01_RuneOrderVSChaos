using System.Collections.Generic;
using UnityEngine;

internal class StartCubeConfigurator : ICubeConfigurator
{
    private readonly List<CubesConfiguration> _configurations = new();

    internal StartCubeConfigurator()
    {
        _configurations.Add(new LineOfFourCubes());
        _configurations.Add(new LineOfFourCubes());
        _configurations.Add(new LineOfFourCubes());
        _configurations.Add(new SquareOfNineCubes());
        _configurations.Add(new SquareOfNineCubes());
        _configurations.Add(new LConfiguration());
    }

    public List<LocalPosition> CreateConfiguration()
    {
        int index = Random.Range(0, _configurations.Count);

        return _configurations[index].GenerateConfiguration();
    }
}