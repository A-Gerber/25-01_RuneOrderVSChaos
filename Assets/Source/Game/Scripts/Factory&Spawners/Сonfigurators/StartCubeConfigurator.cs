using System.Collections.Generic;
using UnityEngine;

internal class StartCubeConfigurator : ICubeConfigurator
{
    private readonly List<CubesConfiguration> _configurations = new();

    internal StartCubeConfigurator()
    {      
        _configurations.Add(new LineOfThreeCubes());
        _configurations.Add(new LineOfThreeCubes());
        _configurations.Add(new LineOfThreeCubes());
        _configurations.Add(new AngleOfThreeCubes());
        _configurations.Add(new AngleOfThreeCubes());
        _configurations.Add(new AngleOfThreeCubes());
        _configurations.Add(new DiagonalOfTwoCubes());
        _configurations.Add(new DiagonalOfTwoCubes());
        _configurations.Add(new DiagonalOfTwoCubes());
        _configurations.Add(new DiagonalOfThreeCubes());
        _configurations.Add(new LineOfFourCubes());
        _configurations.Add(new SquareOfNineCubes());
    }

    public List<LocalPosition> CreateConfiguration()
    {
        int index = Random.Range(0, _configurations.Count);

        return _configurations[index].GenerateConfiguration();
    }
}