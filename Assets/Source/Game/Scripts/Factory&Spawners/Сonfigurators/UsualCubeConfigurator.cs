using System.Collections.Generic;
using UnityEngine;

internal class UsualCubeConfigurator : ICubeConfigurator
{
    private readonly List<CubesConfiguration> _configurations = new();

    internal UsualCubeConfigurator()
    {
        _configurations.Add(new LineOfThreeCubes());
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
        _configurations.Add(new DiagonalOfThreeCubes());
        _configurations.Add(new DiagonalOfThreeCubes());
        _configurations.Add(new LineOfFourCubes());
        _configurations.Add(new LineOfFourCubes());
        _configurations.Add(new SquareOfNineCubes());
        _configurations.Add(new LConfiguration());
        _configurations.Add(new TConfiguration());
        _configurations.Add(new AngleOfFiveCubes());
    }

    public List<LocalPosition> CreateConfiguration()
    {
        int index = Random.Range(0, _configurations.Count);

        return _configurations[index].GenerateConfiguration();
    }
}