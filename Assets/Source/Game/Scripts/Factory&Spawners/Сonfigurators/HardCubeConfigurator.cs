using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class HardCubeConfigurator : ICubeConfigurator
    {
        private readonly List<CubesConfiguration> _configurations = new();

        internal HardCubeConfigurator()
        {
            /*
            _configurations.Add(new CConfiguration());
            _configurations.Add(new LConfiguration());
            _configurations.Add(new LConfiguration());
            _configurations.Add(new LineOfFiveCubes());
            _configurations.Add(new LineOfFiveCubes());
            _configurations.Add(new LineOfFourCubes());
            _configurations.Add(new SquareOfNineCubes());
            _configurations.Add(new SquareOfNineCubes());
            _configurations.Add(new SquareOfSixteenCubes());
            _configurations.Add(new TConfiguration());
            _configurations.Add(new TConfiguration());
            _configurations.Add(new ZConfiguration());*/
            _configurations.Add(new Gigant());
        }

        public List<LocalPosition> CreateConfiguration()
        {
            int index = Random.Range(0, _configurations.Count);

            return _configurations[index].GenerateConfiguration();
        }
    }
}