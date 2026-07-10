using System.Collections.Generic;
using UnityEngine;

internal class VeryHardCubeConfigurator : ICubeConfigurator
{
    private readonly List<CubesConfiguration> _configurations = new();

    internal VeryHardCubeConfigurator()
    {
        _configurations.Add(new LineOfThreeCubes());
        _configurations.Add(new LineOfFourCubes());
        _configurations.Add(new AngleOfThreeCubes());
        _configurations.Add(new DiagonalOfThreeCubes());
        _configurations.Add(new DiagonalOfThreeCubes());
        _configurations.Add(new DiagonalOfFourCubes());
        _configurations.Add(new SquareOfNineCubes());
        _configurations.Add(new SquareOfNineCubes());
        _configurations.Add(new LConfiguration());
        _configurations.Add(new LConfiguration());
        _configurations.Add(new TConfiguration());
        _configurations.Add(new TConfiguration());
        _configurations.Add(new ZConfiguration());
        _configurations.Add(new ZConfiguration());
        _configurations.Add(new AngleOfFiveCubes());
        _configurations.Add(new AngleOfFiveCubes());
    }

    public List<LocalPosition> CreateConfiguration()
    {
        int index = Random.Range(0, _configurations.Count);

        return _configurations[index].GenerateConfiguration();
    }
}