using System.Collections.Generic;
using UnityEngine;

internal class SimpleCubeConfigurator : ICubeConfigurator
{
    private readonly List<CubesConfiguration> _configurations = new();

    internal SimpleCubeConfigurator()
    {
        //_configurations.Add(new LineOfFourCubes());
        //_configurations.Add(new LineOfFourCubes());
        //_configurations.Add(new LineOfFourCubes());
        //_configurations.Add(new SquareOfNineCubes());
       //_configurations.Add(new SquareOfNineCubes());
        //_configurations.Add(new LConfiguration());
        _configurations.Add(new SquareOfSixteenCubes());
    }

    public List<LocalPosition> CreateConfiguration()
    {
        int index = Random.Range(0, _configurations.Count);

        return _configurations[index].GenerateConfiguration();
    }
}